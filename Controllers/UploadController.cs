using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Market.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Market.Models;

namespace Market.Controllers
{
    [ApiController]
    [Authorize(Roles = "User")]
    [Route("upload")]
    public class UploadController : ControllerBase
    {
        private readonly IFileService _fileService;

        public UploadController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpPost("image")]
        [RequestSizeLimit(2147483647)]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File is empty.");

            var fileName = await _fileService.UploadImageAsync(file);
            if (fileName.Code != 1)
                return Ok(fileName);
            else
                return Ok(Result<string>.Ok("http://localhost:8080/api/image/" + fileName.Data));
        }


        [HttpGet("image/delete")]
        public async Task<IActionResult> DeleteImage([FromQuery] string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return BadRequest("File name is required.");

            await _fileService.RemoveImageAsync(fileName);
            return Ok();

        }
    }
}