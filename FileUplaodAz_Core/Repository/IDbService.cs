using FileUplaodAz_Core.Models;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace FileUplaodAz_Core.Repository
{
    public interface IDbService
    {
        public void rEntry(BlobRecord blob);
        public string delete(int? id);
        public void Save();
    }
}
