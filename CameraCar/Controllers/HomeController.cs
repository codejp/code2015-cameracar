using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace CameraCar.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult gallery()
        {
            var saveDir = Server.MapPath("~/App_Data");
            var files = System.IO.Directory
                .GetFiles(saveDir, "photo*.jpg")
                .OrderBy(name => name);

            return View(files);
        }

        public ActionResult photo(string id)
        {
            if (Regex.IsMatch(id ?? "", @"^photo\d{3}$") == false)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            var saveDir = Server.MapPath("~/App_Data");
            var imagPath = System.IO.Path.Combine(saveDir, id + ".jpg");

            return File(imagPath, "image/jpeg");
        }
    }
}