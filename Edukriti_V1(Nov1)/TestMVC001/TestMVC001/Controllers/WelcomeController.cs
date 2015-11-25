﻿using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace TestMVC001.Controllers
{
    public class WelcomeController : Controller
    {
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}