using RoundTheCode.ByteTurn.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RoundTheCode.ByteTurn.Web.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase FileUpload)
        {
            var allowedFileExtensions = new List<string>();
            allowedFileExtensions.Add("jpg");
            allowedFileExtensions.Add("gif");
            allowedFileExtensions.Add("png");

            ListingService.WebUpload(HttpContext, FileUpload, @"C:\Users\David\Projects\RoundTheCode.ByteTurn\branches\20140205\RoundTheCode.ByteTurn.Web", allowedFileExtensions);

            return View();
        }

    }
}
