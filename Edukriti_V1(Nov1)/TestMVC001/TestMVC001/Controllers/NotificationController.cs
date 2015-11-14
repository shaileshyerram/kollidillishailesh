
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using TestMVC001.Core.Models;
using TestMVC001.Core.Services;
using TestMVC001.Models;

namespace TestMVC001.Controllers
{
    public class NotificationController : Controller
    {
        // GET: Notification
        public ActionResult Index()
        {
            return View();
        }

        [Route("sendnotification")]
        [Authorize(Roles ="Admin")]
        public JsonResult SendNotification(NotificationModel model)
        {
            string ConnectionString = WebConfigurationManager.ConnectionStrings["myConnectionString"].ToString();
            String NotificationSender = "School Admin";
            String NotificationCategory = "Test";
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                string query = "INSERT INTO[dbo].[tblNotification] ([PhoneNumber],[Sender],[Message],[Category]) VALUES('" + model.ToPhoneNumber + "', '" + NotificationSender + "',  '" + model.Message + "',   '" + NotificationCategory + "') ";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.ExecuteNonQuery();
                con.Close();
            }
            var smsResponseModel = SendSmsService.SendNotification(model.ToPhoneNumber, model.Message);
            AttendanceService.InsertSmsResponse(smsResponseModel);
            return new JsonResult { Data = "Message sent", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

    }
}
