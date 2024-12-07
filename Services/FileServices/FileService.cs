using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Market.Interfaces;

namespace Market.Services
{
    public class FileService : IFileService
    {
        private readonly string _imagePath = Path.Combine(Directory.GetCurrentDirectory(), "images");

        public async Task<string> UploadImageAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is empty.");

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var fullPath = Path.Combine(_imagePath, fileName);

            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return fileName;
        }
    }
}