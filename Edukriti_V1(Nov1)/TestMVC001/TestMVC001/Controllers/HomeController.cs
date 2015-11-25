using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace TestMVC001.Controllers
{
    [Authorize(Roles = "Admin")]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}