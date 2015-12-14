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
            /*string smsUrl = String.Format("{0}&mobileno={1}&msg={2}",
                //WebConfigurationManager.AppSettings["BulkSMSBaseUrl"], responseModel.PhoneNumber, msg);*/
            string smsUrl = String.Format("{0}&from={1}&to={2}&msg={3}",
                //WebConfigurationManager.AppSettings["BulkSMSBaseUrl"],"EduKriti", responseModel.PhoneNumber, msg);
                WebConfigurationManager.AppSettings["BulkSMSBaseUrl"], "SCHOOL", "9063713741", msg);
            var client = new WebClient();
            var response = client.DownloadString(smsUrl);
            var status = "Failure";
            if (!string.IsNullOrEmpty(response) && response.IsNumeric())
            {
                status = "Success";
            }
            return new SmsResponseModel
            {
                Response = response,
                SmsUrl = smsUrl,
                Status = status
            };
        }
        public static SmsResponseModel SendNotification(string phoneNumber, string message)
        {
            string smsUrl = String.Format("{0}&from={1}&to={2}&msg={3}",WebConfigurationManager.AppSettings["BulkSMSBaseUrl"], "SCHOOL", phoneNumber, message);

            //string smsUrl = String.Format("{0}&from={1}&to={2}&msg={3}", "http://trans.rishiitsolutions.com/API/sms.php?username=javeed&password=javeed@1234&type=1&dnd_check=0", "SCHOOL", phoneNumber, message);

            var client = new WebClient();
            var response = client.DownloadString(smsUrl);
            var status = "Failure";
            if (!string.IsNullOrEmpty(response) && response.IsNumeric())
            {
                status = "Success";
            }
            return new SmsResponseModel
            {
                Response = response,
                SmsUrl = smsUrl,
                Status = status
            };
        }

        public static SmsResponseModel SendSmsDailyReport(ReportResponseModelWithContacts reportResponseModelWithContacts)
        {

            //msg = "Sravani Janapally Date : 11 Dec 2015 In Time : 8:26: AM Out Time : 3:08: PM"
            var msg = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}", reportResponseModelWithContacts.Name,"'s Attendance for", " Date : ", reportResponseModelWithContacts.AttendanceDate, ",",
                " In Time : ", reportResponseModelWithContacts.InTime, 
                ",",
                " Out Time : ", reportResponseModelWithContacts.OutTime);

            string smsUrl = String.Format("{0}&from={1}&to={2}&msg={3}",
                WebConfigurationManager.AppSettings["BulkSMSBaseUrl"], "SCHOOL", "9063713741", msg);
            var client = new WebClient();
            var response = client.DownloadString(smsUrl);
            var status = "Failure";
            if (!string.IsNullOrEmpty(response) && response.IsNumeric())
            {
                status = "Success";
            }
            return new SmsResponseModel
            {
                Response = response,
                SmsUrl = smsUrl,
                Status = status
            };

        }
    }
    public static class Extension
    {
        public static bool IsNumeric(this string s)
        {
            float output;
            return float.TryParse(s, out output);
        }
    }


}
