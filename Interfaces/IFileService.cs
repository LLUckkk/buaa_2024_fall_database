using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Market.Interfaces
{
    public interface IFileService
    {
        Task<string> UploadImageAsync(IFormFile file);
        Task<bool> RemoveImageAsync(string fileName);
    }
}