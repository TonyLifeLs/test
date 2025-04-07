using backend_grade_pro.src.Data;
using backend_grade_pro.src.models;
using Microsoft.AspNetCore.Mvc;


namespace backend_grade_pro.src.controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly string _photoDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "photos");

        public PhotosController(ApplicationDbContext context)
        {
            _context = context;

            if (!Directory.Exists(_photoDirectory))
            {
                Directory.CreateDirectory(_photoDirectory);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UploadPhoto(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            var filePath = Path.Combine(_photoDirectory, file.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var photo = new Photo
            {
                Url = $"/photos/{file.FileName}"
            };

            _context.Photos.Add(photo);
            await _context.SaveChangesAsync();

            return Ok(new { FilePath = photo.Url });
        }
    }
}
