using Microsoft.AspNetCore.Mvc;
namespace Market.Controllers
{
    [Route("image")]
    public class ImageController : Controller
    {
        [HttpGet("{filename}")]
        public IActionResult GetImage([FromRoute] string filename)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "images", filename);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            var image = System.IO.File.OpenRead(filePath);
            return File(image, "image/jpeg");
        }

        [HttpGet("{path}/{filename}")]
        public IActionResult GetImage([FromRoute] string path, [FromRoute] string filename)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "images", path, filename);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            var image = System.IO.File.OpenRead(filePath);
            return File(image, "image/jpeg");
        }
    }
}