using FileUplaodAz_Core.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Threading.Tasks;

namespace FileUplaodAz_Core.Repository
{
    public class DbService : IDbService
    {
        private readonly azblobstorageContext _dbContext;
        public DbService()
        {
            _dbContext = new azblobstorageContext();
        }
        public DbService(azblobstorageContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void rEntry(BlobRecord blob)
        {
            _dbContext.BlobRecord.Add(blob);
            _dbContext.SaveChanges();
        }
        public void delete(int id)
        {
            BlobRecord blobRecord = _dbContext.BlobRecord.Find(id);
            _dbContext.BlobRecord.Remove(blobRecord);
            //var check = await AzureBlobClient.DeleteBlob(blobRecord.url);
            //if (check == false)
            //{
            //    ViewBag.status = 400;
            //}
            _dbContext.SaveChanges();
        }
    }
}
