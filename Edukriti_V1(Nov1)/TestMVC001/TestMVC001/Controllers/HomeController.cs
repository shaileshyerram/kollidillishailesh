using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Net;
using System.Data;

namespace TestMVC001.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //TODO check if request.queryString.count > 1 in any scenario
            if (Request.QueryString.Count > 0)
            {
                //sample URL http://localhost:62206/?$45610&99&0000009999&10092015114300,0000009999&11092015114800
                string queryString = Request.QueryString[0].ToString(CultureInfo.InvariantCulture);
                string[] qsParameters = queryString.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries); //TODO check max how many sub-requests allowed in one request - change to list
                if (qsParameters.Length >= 4)
                {
                    string orgId = RemoveSpecialChars(qsParameters[0]);
                    string machineId = qsParameters[1];
                    var requestModelList = new List<RequestModel>();

                    for (int index = 2; index < qsParameters.Length; index++)
                    {
                        string rfId = qsParameters[index].Trim();

                        index++;
                        string dtAttendance = RemoveSpecialChars(qsParameters[index]);
                        DateTime dateTimeAttendance = DateTime.ParseExact(dtAttendance, "ddMMyyyyHHmmss", CultureInfo.InvariantCulture);
                        var requestModel = new RequestModel
                        {
                            orgId = orgId,
                            machineId = machineId,
                            rfId = rfId,
                            dtAttendance = dateTimeAttendance
                        };
                        requestModelList.Add(requestModel);
                    }
                    foreach (var requestModel in requestModelList)
                    {
                        if (!String.IsNullOrEmpty(orgId) && !String.IsNullOrEmpty(machineId)
                            && !String.IsNullOrEmpty(requestModel.rfId) && requestModel.rfId.Length > 0 && requestModel.rfId.Length <= 16
                            && requestModel.dtAttendance != null)
                        {
                            int rfidInt = int.Parse(requestModel.rfId);
                            string connectionString = WebConfigurationManager.ConnectionStrings["myConnectionString"].ToString();
                            using (var con = new SqlConnection(connectionString))
                            {
                                //Insert student attendance record and get the student details to send the SMS
                                con.Open();
                                var cmd1 = new SqlCommand("InsertStudentAttendance", con)
                                {
                                    CommandType = CommandType.StoredProcedure
                                };
                                cmd1.Parameters.AddWithValue("@rfid", rfidInt);
                                cmd1.Parameters.AddWithValue("@machineId", machineId);
                                cmd1.Parameters.AddWithValue("@orgId", orgId);
                                cmd1.Parameters.AddWithValue("@attendanceDateTime", requestModel.dtAttendance);
                                cmd1.Parameters.Add("@phoneNumber", SqlDbType.Float);
                                cmd1.Parameters["@phoneNumber"].Direction = ParameterDirection.Output;
                                cmd1.Parameters.Add("@studentName", SqlDbType.VarChar, 765);
                                cmd1.Parameters["@studentName"].Direction = ParameterDirection.Output;
                                cmd1.ExecuteReader();
                                con.Close();

                                //Sending SMS using Bulk Service
                                string mobileNo = cmd1.Parameters["@PhoneNumber"].Value.ToString(); //"9966770761"  //9030644017;
                                string studentName = cmd1.Parameters["@studentName"].Value.ToString();
                                if (!String.IsNullOrEmpty(mobileNo))
                                {
                                    var msg = studentName + " has reached the campus at " + requestModel.dtAttendance;
                                    string smsUrl = String.Format("{0}&mobileno={1}&msg={2}", WebConfigurationManager.AppSettings["BulkSMSBaseUrl"], mobileNo, msg);
                                    var client = new WebClient();
                                    var response = client.DownloadString(smsUrl);

                                    //Store the SMS result in the database
                                    con.Open();
                                    var cmnd2 = new SqlCommand("InsertSMSResponse", con)
                                    {
                                        CommandType = CommandType.StoredProcedure
                                    };
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
            var chars = new[] { "$", "#", "*" };
            if (str != null)
            {
                foreach (string t in chars)
                {
                    if (str.Contains(t))
                    {
                        str = str.Replace(t, "");
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
    public DateTime dtAttendance;
}
