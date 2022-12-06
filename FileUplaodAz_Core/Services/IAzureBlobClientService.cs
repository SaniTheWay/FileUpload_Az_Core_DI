using FileUplaodAz_Core.Models;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace FileUplaodAz_Core.Services
{
    public interface IAzureBlobClientService
    {
         Task<BlobRecord> UploadBlob(IFormFile file);
         Task<bool> DeleteBlob(string path);
         Task<int> CheckFile(IFormFile ufile);
    }
}
