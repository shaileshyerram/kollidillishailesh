using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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

        [HttpPost]
        [Authorize(Roles ="Admin")]
        public ActionResult ProcessNotification(NotificationModel model)
        {
            static readonly string ConnectionString = WebConfigurationManager.ConnectionStrings["myConnectionString"].ToString();
            String NotificationSender = "School Admin";
            String NotificationCategory = "Test";
            using (var con = new SqlConnection(ConnectionString))
            {
                //Insert student attendance record and get the student details to send the SMS
                con.Open();

                string query = "INSERT INTO[dbo].[tblNotification] ([PhoneNumber],[Sender],[Message],[Category]) VALUES('" + model.ToPhoneNumber + "', '" + NotificationSender + "',  '" + model.NotificationText + "',   '" + NotificationCategory + "') ";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.ExecuteNonQuery();
                con.Close();

                var attendanceResponseModel = new AttendanceResponseModel
                {
                    PhoneNumber = cmd1.Parameters["@PhoneNumber"].Value.ToString(),
                    StudentName = cmd1.Parameters["@studentName"].Value.ToString(),
                    IsInTime = Convert.ToBoolean(cmd1.Parameters["@isInTime"].Value)
                };
                return attendanceResponseModel;
            }
            // get parameters
            // store in db
            //INSERT INTO[dbo].[tblNotification] ([PhoneNumber],[Sender],[Message],[Category]) VALUES(9030644017,'Admin','Test Notification Message','Admin')




            // call smssendservice

            return Content ("Sucessfully Sent Notification");


        }

    }
}
