using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc;
using TestMVC001.Core.Models;
using TestMVC001.Core.Services;
using TestMVC001.Models;

//AMS - Attendance Message/Management Service

namespace TestMVC001.Controllers
{
    public class AMSController : Controller
    {
        public ActionResult Index()
        {
            //TODO check if request.queryString.count > 1 in any scenario
            if (Request.QueryString.Count > 0)
            {
                //sample URL http://localhost:62206/ams?$99999&99&5780786&15112015114300,5780786&15112015184800
                //sample URL http://localhost:62206/ams?$99999&OutMessages
                //Date format is ddmmyyyy
                string queryString = Request.QueryString[0].ToString(CultureInfo.InvariantCulture);

                if (queryString.Contains("OutMessages"))
                {
                    string[] qsParameters = queryString.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    string orgId = RemoveSpecialChars(qsParameters[0]);
                    DateTime outReportDate = DateTime.Today;

                    var reportRequestModel = new ReportRequestModel
                    {
                        DtFrom = outReportDate.AddDays(-2),
                        DtTo = outReportDate.AddDays(-2)
                    };
                    var reportResponseModelWithContactsList = ReportService.GetAttendanceReportWithContacts(reportRequestModel);

                    foreach (var reportResponseModelWithContacts in reportResponseModelWithContactsList)
                    {
                        var phoneNumber = reportResponseModelWithContacts.PhoneNumber;
                        //Sending SMS using Bulk Service
                        if (!String.IsNullOrEmpty(phoneNumber))
                        {
                             var smsResponseModel = SendSmsService.SendSmsDailyReport(reportResponseModelWithContacts);
                             AttendanceService.InsertSmsResponse(smsResponseModel);
                        }

                    }

                }
                else
                {
                    string[] qsParameters = queryString.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries); //TODO check max how many sub-requests allowed in one request - change to list
                    if (qsParameters.Length >= 4)
                    {
                        string orgId = RemoveSpecialChars(qsParameters[0]);
                        string machineId = qsParameters[1];
                        var requestModelList = new List<AttendanceRequestModel>();

                        for (int index = 2; index < qsParameters.Length; index++)
                        {
                            string rfId = qsParameters[index].Trim();

                            index++;
                            string dtAttendance = RemoveSpecialChars(qsParameters[index]);
                            DateTime dateTimeAttendance = DateTime.ParseExact(dtAttendance, "ddMMyyyyHHmmss", CultureInfo.InvariantCulture);
                            var requestModel = new AttendanceRequestModel
                            {
                                OrgId = orgId,
                                MachineId = machineId,
                                RfId = rfId,
                                DtAttendance = dateTimeAttendance
                            };
                            requestModelList.Add(requestModel);
                        }
                        foreach (var requestModel in requestModelList)
                        {
                            /*if (!String.IsNullOrEmpty(orgId) && !String.IsNullOrEmpty(machineId)
                                && requestModel.RfId != null && !String.IsNullOrEmpty(requestModel.RfId) && requestModel.RfId.Length > 0 && requestModel.RfId.Length <= 16
                                && requestModel.DtAttendance != null)//TODO check default value of datetime.parseexact*/

                            //Insert student attendance record and get the student details to send the SMS
                            var attendanceResponseModel = AttendanceService.InsertOrUpdateAttendanceRecord(requestModel);

                            //Sending SMS using Bulk Service
                            if (!String.IsNullOrEmpty(attendanceResponseModel.PhoneNumber))
                            {
                                var smsResponseModel = SendSmsService.SendSms(attendanceResponseModel, requestModel.DtAttendance);
                                AttendanceService.InsertSmsResponse(smsResponseModel);
                            }

                            //query To get the value from table tblregistration
                            //string selectquery = "Select * from tblregistration where UserId='" + rfId + "' ";
                            // TODO ==> Identify In and Out Timestamps. as of now,  morning 6 AM to 10 AM ==> IN Time , evening 3 to 6 ==>  OUT Time
                            // TODO ==> think of correct data model to maintain this data
                            // TODO ==> fix RFID datatype in database.10 digits.
                            // TODO ==> Make sure RFID is assigned to every student during registration. registration page of UI.

                            if (requestModel.RfId != null)
                            {
                                //For Successfull Insertion Of Data Into database We are giving response To the device
                                //return View(requestModel);
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
    }
}