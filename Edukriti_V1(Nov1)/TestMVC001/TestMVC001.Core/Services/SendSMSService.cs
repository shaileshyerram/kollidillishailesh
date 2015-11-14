using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using TestMVC001.Core.Models;

namespace TestMVC001.Core.Services
{
    public class SendSmsService
    {
        public static SmsResponseModel SendSms(AttendanceResponseModel responseModel, DateTime dtAttendance)
        {
            string subMsg = responseModel.IsInTime ? " has entered the campus at " : " has left the campus at ";
            var msg = string.Format("{0}{1}{2}", responseModel.StudentName, subMsg, dtAttendance);
            string smsUrl = String.Format("{0}&mobileno={1}&msg={2}",
                //WebConfigurationManager.AppSettings["BulkSMSBaseUrl"], responseModel.PhoneNumber, msg);
                WebConfigurationManager.AppSettings["BulkSMSBaseUrl"], "9063713741", msg);
            var client = new WebClient();
            var response = client.DownloadString(smsUrl);
            return new SmsResponseModel
            {
                Response = response,
                SmsUrl = smsUrl
            };
        }
        public static SmsResponseModel SendNotification(string phoneNumber, string message)
        {
            string smsUrl = String.Format("{0}&mobileno={1}&msg={2}",
                WebConfigurationManager.AppSettings["BulkSMSBaseUrl"], phoneNumber, message);
            var client = new WebClient();
            var response = client.DownloadString(smsUrl);
            return new SmsResponseModel
            {
                Response = response,
                SmsUrl = smsUrl
            };
        }
    }
}
