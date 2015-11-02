using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Text;
using System.Net;

namespace TestMVC001.Controllers
{

    public class HomeController : Controller
    {
        string ConnectionString = WebConfigurationManager.ConnectionStrings["myConnectionString"].ToString();
        string rfId;//User Id
        int rfidInt; // rfid in integer format
        string tot;//Time Of Attendance
        public ActionResult Index()
        {
            string[] querystring = new string[20];
            for (int i = 0; i < Request.QueryString.Count; i++)
            {
                string Qstr = Request.QueryString[i].ToString();
                //  Split the querystring
                querystring = Qstr.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            }
            if (querystring[2] != null)
            {
                for (int index = 2; index < querystring.Length; index++)
                {
                    string OrgId = querystring[0];
                    //Removing special charecters Like * ,$ form Organization Id
                    string newOrgId = RemoveSpecialChars(OrgId);
                    string machineId = querystring[1];
                    string uname;
                    String studentId;
                    String studentName;
                    // TODO ==> fix RFID datatype in database.10 digits.
                    // TODO ==> Make sure RFID is assigned to every student during registration. registration page of UI.
                    // TODO ==> Convert all the SQLs in this page to PROC
                    rfId = querystring[index];
                        Console.WriteLine("raw rfid before conversion "+rfId);
                    rfidInt = int.Parse(rfId);
                    Console.WriteLine("numeric rfid after conversion " + rfidInt);
                    tot = querystring[index + 1];
                    string tot1 = RemoveSpecialChars(tot);
                    index = index + 1;
                    if (tot != null && machineId != "" && rfId.Length <= 16 && rfId.Length > 0)
                    {
                        if (tot1.Length == 14)
                        {
                            //Coerting Value Into DateTime
                            string format = "ddMMyyyyHHmmss";
                            DateTime dateTime = DateTime.ParseExact(tot1, format,
                                System.Globalization.CultureInfo.InvariantCulture);
                            String result = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
                            tot = result;

                            using (SqlConnection con = new SqlConnection(ConnectionString))
                            {
                                con.Open();
                                //query To get the value from table tblregistration
                                //string selectquery = "Select * from tblregistration where UserId='" + rfId + "' ";
                                string selectquery = "Select * from tblStudent where rfId='" + rfidInt + "' ";
                                SqlCommand command1 = new SqlCommand(selectquery, con);
                                using (SqlDataReader rdr = command1.ExecuteReader())
                                {
                                    if (rdr.HasRows)
                                    {
                                        if (rdr.Read())
                                        {
                                            // TODO ==> fetch student details. Student ID and Parent Phone Number.

                                            studentId = rdr["StudentId"].ToString();
                                            studentName = rdr["StudentFirstName"].ToString() + " " + rdr["StudentMiddleName"].ToString() + " " + rdr["StudentLastName"].ToString();


                                            con.Close();
                                            con.Open();

                                            // TODO ==> Insert attendance record with appropriate student ID associated.
                                            // TODO ==> Identify In and Out Timestamps. as of now,  morning 6 AM to 10 AM ==> IN Time , evening 3 to 6 ==>  OUT Time
                                            // TODO ==> think of correct data model to maintain this data
                                            string query = "insert into tblStudentAttendance (StudentId,InTime,OutTime,MachineId,RFID,StudentName,OrgId) Values ('" + studentId + "', '" + tot + "',  '" + tot + "',   '" + machineId + "','" + rfId + "','" + studentName + "','" + newOrgId + "') ";
                                            SqlCommand cmd = new SqlCommand(query, con);
                                            cmd.ExecuteNonQuery();

                                            // TODO ==> send  SMS to corresponding Parent.
                                            //Sending SMS using Bulk Service
                                            string bulkUrl = "http://tra.bulksmshyderabad.co.in/websms/sendsms.aspx?userid=demosms&password=123456&sender=INTECH&";
                                            //string mobileNo = "9030644017";
                                            string mobileNo = "9966770761";
                                            string rfidString = rfId;

                                            StringBuilder sb = new StringBuilder();
                                            sb.Append(bulkUrl);
                                            sb.Append("mobileno=" + mobileNo);
                                            sb.Append("&msg=" + rfidString + "uname=" + studentName);


                                            var client = new WebClient();
                                            var content = client.DownloadString(sb.ToString());

                                            //Store the SMS result in database
                                            string smsInsertQuery = "insert into tblSMSRequestResponse (RequestURL,ResponseText,TransmissionStatus) Values('" + sb.ToString() + "'," + "'" +  content + "'" + ",'" + content.Substring(0, content.IndexOf(':') - 1) + "') ";
                                            SqlCommand cmd2 = new SqlCommand(smsInsertQuery, con);
                                            cmd2.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {

                                        //If The User Name Doesn't Exist In The Database We Will Insert as Unknown          
                                        uname = "UnKnown";
                                        con.Close();
                                        con.Open();
                                        string query = "insert into tblattendance (OrgId,MachineId,UserId,UserName,DateOfTransaction)Values('" + newOrgId + "'," + machineId + ",'" + rfId + "','" + uname + "','" + tot + "') ";
                                        SqlCommand cmd = new SqlCommand(query, con);
                                        cmd.ExecuteNonQuery();

                                        //Sending SMS using Bulk Service
                                        string bulkUrl = "http://tra.bulksmshyderabad.co.in/websms/sendsms.aspx?userid=demosms&password=123456&sender=INTECH&";
                                        //string mobileNo = "9030644017";
                                        string mobileNo = "9966770761";
                                        string rfidString = rfId;

                                        StringBuilder sb = new StringBuilder();
                                        sb.Append(bulkUrl);
                                        sb.Append("mobileno=" + mobileNo);
                                        sb.Append("&msg=" + rfidString + "uname=" + uname);


                                        var client = new WebClient();
                                        var content = client.DownloadString(sb.ToString());

                                        //Store the SMS result in database
                                        string smsInsertQuery = "insert into tblSMSRequestResponse (RequestURL,ResponseText,TransmissionStatus) Values('" + sb.ToString() + "'," + "'" + content + "'" + ",'" + content.Substring(0, content.IndexOf(':') - 1) + "') ";
                                        SqlCommand cmd2 = new SqlCommand(smsInsertQuery, con);
                                        cmd2.ExecuteNonQuery();
                                    }
                                }
                            }
                        }

                    }
                }
                if (rfId != null)
                {
                    //For Successfull Insertion Of Data Into database We are giving response To the device
                    Response.Write("$RFID=0#");
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