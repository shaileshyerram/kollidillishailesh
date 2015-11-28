using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace TestMVC001.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}