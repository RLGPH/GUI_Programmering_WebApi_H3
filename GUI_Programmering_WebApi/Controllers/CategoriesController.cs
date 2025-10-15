using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GUI_Programmering_WebApi.Models;
using Mapster;

namespace GUI_Programmering_WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public CategoriesController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryWithIdDTO>>> GetCategories()
        {
            var categories = await _context.Categories
                .Include(c => c.Products) 
                .ToListAsync();

            var dtos = categories.Adapt<List<CategoryWithIdDTO>>();
            return Ok(dtos);
        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryWithIdDTO>> GetCategory(int id)
        {
            var category = await _context.Categories
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.CategoryId == id);

            if (category == null)
                return NotFound();

            var dto = category.Adapt<CategoryWithIdDTO>();
            return Ok(dto);
        }

        // PUT: api/Categories/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(int id, CategoryWithIdDTO dto)
        {
            if (id != dto.CategoryId)
                return BadRequest();

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return NotFound();

            dto.Adapt(category);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/Categories
        [HttpPost]
        public async Task<ActionResult<CategoryWithIdDTO>> PostCategory(CategoryDTO dto)
        {
            var category = dto.Adapt<Category>();
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            var resultDto = category.Adapt<CategoryWithIdDTO>();
            return CreatedAtAction(nameof(GetCategory), new { id = category.CategoryId }, resultDto);
        }

        // DELETE: api/Categories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return NotFound();

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
