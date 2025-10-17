using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GUI_Programmering_WebApi.Models;
using Mapster;

namespace GUI_Programmering_WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public ProductsController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductWithImageDTO>>> GetProducts()
        {
            var products = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Image)
                .ToListAsync();

            // Map each product safely, including null checks
            var productDtos = products.Select(product => new ProductWithImageDTO
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                ProductDescription = product.ProductDescription,
                ProductPrice = product.ProductPrice,
                Category = product.Category != null ? product.Category.Adapt<CategoryDTO>() : null,
                Image = product.Image != null ? product.Image.Adapt<ImageDTO>() : null
            }).ToList();

            return Ok(productDtos);
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductWithImageDTO>> GetProduct(int id)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Image)
                .FirstOrDefaultAsync(p => p.ProductId == id);

            if (product == null) return NotFound();

            var dto = new ProductWithImageDTO
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                ProductDescription = product.ProductDescription,
                ProductPrice = product.ProductPrice,
                Category = product.Category != null ? product.Category.Adapt<CategoryDTO>() : null,
                Image = product.Image != null ? product.Image.Adapt<ImageDTO>() : null
            };

            return Ok(dto);
        }

        // GET: api/Products/Category/5
        [HttpGet("Category/{categoryId}")]
        public async Task<ActionResult<IEnumerable<ProductWithRelationDTO>>> GetProductsByCategory(int categoryId)
        {
            var products = await _context.Products
                .Include(p => p.Category)
                .Where(p => p.CategoryId == categoryId)
                .ToListAsync();

            if (!products.Any())
                return NotFound($"No products found for Category ID {categoryId}");

            var productDtos = products.Adapt<List<ProductWithRelationDTO>>();
            return Ok(productDtos);
        }

        // PUT: api/Products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, ProductWithIdDTO dto)
        {
            if (id != dto.ProductId)
                return BadRequest();

            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound();

            dto.Adapt(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/Products
        [HttpPost]
        public async Task<ActionResult<ProductWithIdDTO>> PostProduct(ProductDTO dto)
        {
            var product = dto.Adapt<Product>();
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            var resultDto = product.Adapt<ProductWithIdDTO>();
            return CreatedAtAction(nameof(GetProduct), new { id = product.ProductId }, resultDto);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound();

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
