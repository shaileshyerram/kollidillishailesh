using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Text;
using System.Net;
using System.Data;

namespace TestMVC001.Controllers
{

    public class HomeController : Controller
    {
        string ConnectionString = WebConfigurationManager.ConnectionStrings["myConnectionString"].ToString();

        public ActionResult Index()
        {
            //TODO check if request.queryString.count > 1 in any scenario
            string[] querystring = new string[20]; //TODO check max how many sub-requests allowed in one request - change to list
            if (Request.QueryString.Count > 0)
            {
                string Qstr = Request.QueryString[0].ToString();
                //45610&99&0000009999&10092015114300,0000009999&11092015114800,0000009999&11092015114800
                querystring = Qstr.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (querystring.Length >= 4)
                {
                    string orgId = "";
                    string machineId = "";
                    string rfId;
                    int rfidInt; // rfid in integer format
                    string dtAttendance;
                    orgId = RemoveSpecialChars(querystring[0]);  //Removing special charecters Like * ,$ form Organization Id
                    machineId = querystring[1];
                    var requestModelList = new List<RequestModel>();

                    for (int index = 2; index < querystring.Length; index++)
                    {
                        rfId = querystring[index].Trim();
                        index++;
                        dtAttendance = RemoveSpecialChars(querystring[index]);
                        DateTime dateTime = DateTime.ParseExact(dtAttendance, "ddMMyyyyHHmmss", System.Globalization.CultureInfo.InvariantCulture);
                        dtAttendance = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
                        var requestModel = new RequestModel
                        {
                            orgId = orgId,
                            machineId = machineId,
                            rfId = rfId,
                            dtAttendance = dtAttendance
                        };
                        requestModelList.Add(requestModel);
                    }
                    foreach (var requestModel in requestModelList)
                    {
                        if (!String.IsNullOrEmpty(orgId) && !String.IsNullOrEmpty(machineId)
                            && !String.IsNullOrEmpty(requestModel.rfId) && requestModel.rfId.Length > 0 && requestModel.rfId.Length <= 16
                            && !String.IsNullOrEmpty(requestModel.dtAttendance) && requestModel.dtAttendance.Length == 14)  
                        {
                            rfidInt = int.Parse(requestModel.rfId);
                            using (SqlConnection con = new SqlConnection(ConnectionString))
                            {
                                //Insert student attendance record and get the student details to send the SMS
                                con.Open();
                                SqlCommand cmd1 = new SqlCommand("InsertStudentAttendance", con);
                                cmd1.CommandType = CommandType.StoredProcedure;
                                cmd1.Parameters.AddWithValue("@rfid", requestModel.rfId);
                                cmd1.Parameters.AddWithValue("@machineId", machineId);
                                cmd1.Parameters.AddWithValue("@orgId", orgId);
                                cmd1.Parameters.AddWithValue("@dtAttendance", requestModel.dtAttendance);
                                cmd1.Parameters.Add("@phoneNumber", SqlDbType.Float);
                                cmd1.Parameters["@phoneNumber"].Direction = ParameterDirection.Output;
                                cmd1.Parameters.Add("@studentName", SqlDbType.VarChar, 765);
                                cmd1.Parameters["@studentName"].Direction = ParameterDirection.Output;
                                SqlDataReader rdr = cmd1.ExecuteReader();
                                con.Close();

                                //Sending SMS using Bulk Service
                                string mobileNo = cmd1.Parameters["@PhoneNumber"].Value.ToString(); //"9966770761"  //9030644017;
                                string studentName = cmd1.Parameters["@studentName"].Value.ToString();
                                if (!String.IsNullOrEmpty(mobileNo))
                                {
                                    string baseUrl = "http://tra.bulksmshyderabad.co.in/websms/sendsms.aspx?userid=demosms&password=123456&sender=INTECH";
                                    var msg = studentName + " has reached the campus at " + requestModel.dtAttendance;
                                    string smsUrl = String.Format("{0}&mobileno={1}&msg={2}", baseUrl, mobileNo, msg);
                                    var client = new WebClient();
                                    var response = client.DownloadString(smsUrl);

                                    //Store the SMS result in the database
                                    con.Open();
                                    SqlCommand cmnd2 = new SqlCommand("InsertSMSResponse", con);
                                    cmnd2.CommandType = CommandType.StoredProcedure;
                                    cmnd2.Parameters.AddWithValue("@smsUrl", smsUrl);
                                    cmnd2.Parameters.AddWithValue("@response", response);
                                    cmnd2.Parameters.AddWithValue("@status", response.Substring(0, response.IndexOf(':') - 1));
                                    cmnd2.ExecuteNonQuery();
                                    con.Close();
                                }

                                //query To get the value from table tblregistration
                                //string selectquery = "Select * from tblregistration where UserId='" + rfId + "' ";
                                // TODO ==> Identify In and Out Timestamps. as of now,  morning 6 AM to 10 AM ==> IN Time , evening 3 to 6 ==>  OUT Time
                                // TODO ==> think of correct data model to maintain this data
                                // TODO ==> fix RFID datatype in database.10 digits.
                                // TODO ==> Make sure RFID is assigned to every student during registration. registration page of UI.
                            }
                            if (requestModel.rfId != null)
                            {
                                //For Successfull Insertion Of Data Into database We are giving response To the device
                                Response.Write("$RFID=0#");
                            }
                        }
                    }
                }
            }
            return View();
        }

        //Function for Removing special charecters
        [NonAction]
        public string RemoveSpecialChars(string str)
        {
            string[] chars = new string[] { "$", "#", "*" };
            if (str != null)
            {
                for (int i = 0; i < chars.Length; i++)
                {
                    if (str.Contains(chars[i]))
                    {
                        str = str.Replace(chars[i], "");
                    }
                }
            }
            return str;
        }
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

public class RequestModel
{
    public string orgId;
    public string machineId;
    public string rfId;
    public string dtAttendance;

}