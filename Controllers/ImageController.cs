using Microsoft.AspNetCore.Mvc;
namespace Market.Controllers
{
    [Route("image")]
    public class ImageController : Controller
    {
        [HttpGet("{filename}")]
        public IActionResult GetImage(string filename)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "images", filename.Replace("%2F", "/"));

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            var image = System.IO.File.OpenRead(filePath);
            return File(image, "image/jpeg");
        }
    }
}