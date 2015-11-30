using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TestMVC001.Core.Models;
using TestMVC001.Core.Services;
using System.Web.Security;
using Microsoft.AspNet.Identity;

namespace TestMVC001.Controllers
{
    [Authorize(Roles = "SysAdmin,Admin,Staff,Student")]
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

            //TODO pass ORgID to db
            var sessionUserName = User.Identity.GetUserName();
            var filteredRows = new List<Row>();
            if (User.IsInRole("Student") || ((User.IsInRole("Staff") && reportRequestModel.Category == "Staff")))
            {                
                //List<Row> filteredRows = (from row in reportResponseModel.Rows let rowUserName = row.RowCells[row.RowCells.Count - 1] where !String.IsNullOrEmpty(rowUserName) && !String.IsNullOrEmpty(sessionUserName) && (rowUserName.ToLower() == sessionUserName.ToLower()) select row).ToList();
                foreach (var row in reportResponseModel.Rows)
                {
                    var rowUserName = row.RowCells[row.RowCells.Count - 1];
                    if (!String.IsNullOrEmpty(rowUserName) && !String.IsNullOrEmpty(sessionUserName) && (rowUserName.ToLower() == sessionUserName.ToLower()))
                    {
                        filteredRows.Add(row);
                    }
                }
                reportResponseModel.Rows = filteredRows;
            }
            return new JsonResult { Data = reportResponseModel, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        
	}
}