using Azure.Core;
using GUI_Programmering_WebApi.Models;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GUI_Programmering_WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly IWebHostEnvironment _env;

        public ImagesController(DatabaseContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: api/Images
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ImageDTO>>> GetImages()
        {
            var images = await _context.Images.ToListAsync();
            var dtos = images.Select(img => img.Adapt<ImageDTO>()).ToList();
            return Ok(dtos);
        }

        // GET: api/Images/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ImageDTO>> GetImage(int id)
        {
            var image = await _context.Images.FindAsync(id);
            if (image == null) return NotFound();

            var dto = image.Adapt<ImageDTO>();
            return Ok(dto);
        }

        // POST: api/Images/upload
        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<ImageDTO>> UploadImage([FromForm] ImageUploadRequest request)
        {
            var file = request.File;

            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            var imagesFolder = Path.Combine(_env.WebRootPath ?? "wwwroot", "images");
            if (!Directory.Exists(imagesFolder))
                Directory.CreateDirectory(imagesFolder);

            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(imagesFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var imageUrl = $"/images/{fileName}";

            var image = new Image
            {
                ImageName = file.FileName,
                ImageUrl = imageUrl
            };

            _context.Images.Add(image);
            await _context.SaveChangesAsync();

            var dto = image.Adapt<ImageDTO>();
            return CreatedAtAction(nameof(GetImage), new { id = image.ImageId }, dto);
        }

        // PUT: api/Images/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutImage(int id, ImageDTOWithId dto)
        {
            if (id != dto.ImageId) return BadRequest();

            var image = await _context.Images.FindAsync(id);
            if (image == null) return NotFound();

            image.ImageName = dto.ImageName;
            image.ImageUrl = dto.ImageUrl;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/Images/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteImage(int id)
        {
            var image = await _context.Images.FindAsync(id);
            if (image == null) return NotFound();

            if (!string.IsNullOrEmpty(image.ImageUrl))
            {
                var filePath = Path.Combine(
                    _env.WebRootPath ?? "wwwroot",
                    image.ImageUrl.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString())
                );
                if (System.IO.File.Exists(filePath))
                    System.IO.File.Delete(filePath);
            }

            _context.Images.Remove(image);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
    public class ImageUploadRequest
    {
        public IFormFile File { get; set; } = null!;
    }

}
