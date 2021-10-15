using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YakimaAsrsWeb.Filters;

namespace YakimaAsrsWeb.Controllers
{
    [CustomAuthenticationFilter]
    public class HomeController : Controller
    {
        [CustomAuthorize("User", "SystemAdmin")]
        public ActionResult Index()
        {
            return View();
        }

        [CustomAuthorize("Admin", "SystemAdmin")]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [CustomAuthorize("SystemAdmin")]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult UnAuthorized()
        {
            return View();
        }
    }
}
