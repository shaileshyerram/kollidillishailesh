using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TestMVC001.Controllers
{
    [Authorize(Roles = "Admin")]
    public class WelcomeController : Controller
    {       
        public ActionResult Index()
        {
            return View();
        }
	}
}