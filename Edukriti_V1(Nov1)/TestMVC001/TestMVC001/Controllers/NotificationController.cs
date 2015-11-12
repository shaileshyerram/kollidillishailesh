using System;
using System.Data.SqlClient;
using System.Net;
using System.Text;
using System.Web.Configuration;
using System.Web.Mvc;
using TestMVC001.Models;

namespace TestMVC001.Controllers
{
    public class NotificationController : Controller
    {
        static readonly string ConnectionString = WebConfigurationManager.ConnectionStrings["myConnectionString"].ToString();
        // GET: Notification
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult ProcessNotification(NotificationModel model)
        {

            string NotificationSender = "School Admin";
            String NotificationCategory = "Test";

            using (var con = new SqlConnection(ConnectionString))
            {
                //Insert student attendance record and get the student details to send the SMS
                con.Open();

                string query =
                    "INSERT INTO[dbo].[tblNotification] ([PhoneNumber],[Sender],[Message],[Category]) VALUES('" +
                    model.ToPhoneNumber + "', '" + NotificationSender + "',  '" + model.NotificationText + "',   '" +
                    NotificationCategory + "') ";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.ExecuteNonQuery();
                con.Close();


                string bulkUrl =
                    "http://tra.bulksmshyderabad.co.in/websms/sendsms.aspx?userid=demosms&password=123456&sender=INTECH&";
                string mobileNo = "9030644017";

                StringBuilder sb = new StringBuilder();
                sb.Append(bulkUrl);
                sb.Append("mobileno=" + mobileNo);
                sb.Append("&msg=" + model.NotificationText);

                var client = new WebClient();
                var content = client.DownloadString(sb.ToString());



            }
            return View();
        }
    }
}
