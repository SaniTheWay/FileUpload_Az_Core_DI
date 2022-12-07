using FileUplaodAz_Core.Models;
using FileUplaodAz_Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace FileUplaodAz_Core.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAzureBlobClientService _service;

        public HomeController(IAzureBlobClientService service)
        {
            _service = service;
        }
        
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Upload(IFormFile ufile)
        {
            var status = _service.CheckFile(ufile).Result;
            ViewBag.status = status;
            return View("Index");
        }

        public IActionResult Delete(int? id)
        {
            _service.DeleteBlob();
            return;
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
