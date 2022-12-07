using FileUplaodAz_Core.Models;
using System.IO;
using System.Web;
using System;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using FileUplaodAz_Core.Models;
using FileUplaodAz_Core.Repository;

namespace FileUplaodAz_Core.Services
{
    public class AzureBlobClientService : IAzureBlobClientService
    {
        IDbService _db;
        //private interface IAzureBlobClientService _azureService;

        public AzureBlobClientService(IDbService db)
        {
            _db = db;
        }
        public async Task<int> CheckFile(IFormFile ufile)
        { 
            if (ufile != null)
            {

                try
                {
                    var blob = await UploadBlob(ufile);
                    if(blob != null)
                    {
                        if (blob.Id == 0)
                            _db.rEntry(blob);
                        return 201;
                    }
                    return 413;
                }
                catch{
                    return 409;
                }
            }
            return 400;
        }
        public async Task<BlobRecord> UploadBlob(IFormFile file)
        {
            var connectionStr = "DefaultEndpointsProtocol=https;AccountName=sanidhyastorage;AccountKey=TCRpncR3imOIQwD4QD/t78IeC5dagZVrIoSysbvm/F4hYVVyNn1XP/bqSBNbCKwtI3W/XQR2qVfd+AStU8HMuA==;EndpointSuffix=core.windows.net";
            var containerName = "jamesbond";
            string filename = Guid.NewGuid().ToString() + "_" + file.FileName;
            var client_service = new BlobServiceClient(connectionStr);
            var client_container = client_service.GetBlobContainerClient(containerName);
            var blobclient = client_container.GetBlobClient(@"Sanidhya/" + filename);
            double fsize = file.Length;

            if (fsize > 3145728)
            {
                return null;
            }
            //file.CopyToAsync(blobclient);
            using (var stream = file.OpenReadStream())
            {
                await blobclient.UploadAsync(stream);
            }
            var blob_uri = blobclient.Uri;

            return new BlobRecord(){
                BlobName = Guid.NewGuid().ToString() + "_" + filename,
                Date = DateTime.Now,
                Url = blob_uri.ToString()
            };
        }
        public async int delete(int? id)
        {
            if (id == null)return null;
            _db.delete(id);
            await DeleteBlob()
        }
        //[HttpDelete(nameof(DeleteFile))]
        public async Task<bool> DeleteBlob(string path)
        {
            try
            {
                var connectionStr = "DefaultEndpointsProtocol=https;AccountName=sanidhyastorage;AccountKey=TCRpncR3imOIQwD4QD/t78IeC5dagZVrIoSysbvm/F4hYVVyNn1XP/bqSBNbCKwtI3W/XQR2qVfd+AStU8HMuA==;EndpointSuffix=core.windows.net";
                var containerName = "jamesbond";

                var client_service = new BlobServiceClient(connectionStr);
                var client_container = client_service.GetBlobContainerClient(containerName);

                Uri uri = new Uri(path);
                string filename = Path.GetFileName(uri.LocalPath);

                var blobclient = client_container.GetBlobClient(@"Sanidhya/" + filename);

                return await blobclient.DeleteIfExistsAsync();
            }
            catch
            {
                return false;
            }
        }
    }
}
