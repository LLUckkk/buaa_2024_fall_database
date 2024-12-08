using System.Threading.Tasks;
using Market.Models;
using Microsoft.AspNetCore.Http;

namespace Market.Interfaces
{
    public interface IFileService
    {
        Task<Result<string>> UploadImageAsync(IFormFile file);
        Task<Result> RemoveImageAsync(string fileName);
    }
}