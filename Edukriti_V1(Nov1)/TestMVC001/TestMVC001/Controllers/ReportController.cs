using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestMVC001.Core.Models;
using TestMVC001.Core.Services;

namespace TestMVC001.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ReportController : Controller
    {
        public ActionResult Student()
        {
            return View();
        }
        public ActionResult Staff()
        {
            return View();
        }
       
        [Route("getreport")]
        public JsonResult GetReport(ReportRequestModel reportRequestModel)
        {
            var reportResponseModel = ReportService.GetAttendanceReport(reportRequestModel);
            //return View("_ReportDataPartial", reportResponseModel);
            return new JsonResult { Data = reportResponseModel, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        
	}
}