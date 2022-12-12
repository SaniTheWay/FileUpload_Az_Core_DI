using FileUplaodAz_Core.Models;
using FileUplaodAz_Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
        private readonly IRecaptchaService _recaptcha;
        private readonly IConfiguration _config;

        public HomeController(IAzureBlobClientService service, IRecaptchaService recaptcha, IConfiguration config)
        {
            _config = config;
            _recaptcha = recaptcha;
            _service = service;
        }
        
        public IActionResult Index()
        {
            ViewBag.clientKey = _config.GetSection("Logging:SecretKey:recaptchaClient").Value;
            return View();
        }
        [HttpPost]
        public IActionResult Upload(IFormFile ufile)
        {
            var reqstr = Request.Form["g-recaptcha-response"].ToString();

            if (_recaptcha.RecaptchaRequest(reqstr))
            {
                ViewBag.status = 409;
                return View("Index");
            }
            var status = _service.CheckFile(ufile).Result;
            ViewBag.status = status;
            return View("Index");
        }

        public IActionResult Delete(int? id)
        {
            //_service.DeleteBlob();
            return View();
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
