using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Market.Interfaces;
using Market.Models;
using Market.Constants;

namespace Market.Services
{
    public class FileService(IUserService userService) : IFileService
    {
        private readonly IUserService _userService = userService;
        private readonly string _imagePath = Path.Combine(Directory.GetCurrentDirectory(), "images");

        public async Task<Result<string>> UploadImageAsync(IFormFile file)
        {
            var user = _userService.GetCurrentUser();
            if (user == null)
                return Result<string>.Fail(ResultCode.Fail, "User is not authenticated.");
            if (file == null || file.Length == 0)
                return Result<string>.Fail(ResultCode.Fail, "File is empty.");

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(extension))
                return Result<string>.Fail(ResultCode.Fail, "Unsupported file type.");

            if (file.Length > 5 * 1024 * 1024) // 5 MB limit
                throw new ArgumentException("File size exceeds the limit.");

            var fileName = Guid.NewGuid().ToString() + extension;
            var fullPath = Path.Combine(_imagePath, user.Id, fileName);

            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // return user.Id + "/" + fileName;
            return Result<string>.Ok(user.Id + "/" + fileName);
        }

        public async Task<Result> RemoveImageAsync(string fileName)
        {
            var user = _userService.GetCurrentUser();
            if (user == null)
                return Result.Fail(ResultCode.Fail, "User is not authenticated.");
            if (string.IsNullOrWhiteSpace(fileName))
                return Result.Fail(ResultCode.Fail, "File name is required.");

            var sanitizedFileName = Path.GetFileName(fileName);
            var fullPath = Path.Combine(_imagePath, user.Id, sanitizedFileName);

            if (Path.GetFullPath(fullPath).StartsWith(_imagePath))
            {
                if (File.Exists(fullPath))
                {
                    await Task.Run(() => File.Delete(fullPath));
                    return Result.Ok();
                }
            }
            return Result.Fail(ResultCode.DeleteError, "File not found.");
        }
    }
}