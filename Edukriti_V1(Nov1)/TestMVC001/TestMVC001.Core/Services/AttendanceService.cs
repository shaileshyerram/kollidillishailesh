using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using TestMVC001.Core.Models;


namespace TestMVC001.Core.Services
{
    public class AttendanceService
    {
        static readonly string ConnectionString = WebConfigurationManager.ConnectionStrings["myConnectionString"].ToString();
        public static AttendanceResponseModel InsertOrUpdateAttendanceRecord(AttendanceRequestModel requestModel)
        {
            
            using (var con = new SqlConnection(ConnectionString))
            {
                //Insert student attendance record and get the student details to send the SMS
                con.Open();
                int rfidInt = int.Parse(requestModel.RfId);
                var cmd1 = new SqlCommand("InsertStudentAttendance", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd1.Parameters.AddWithValue("@rfid", rfidInt);
                cmd1.Parameters.AddWithValue("@machineId", requestModel.MachineId);
                cmd1.Parameters.AddWithValue("@orgId", requestModel.OrgId);
                cmd1.Parameters.AddWithValue("@attendanceDateTime", requestModel.DtAttendance);
                cmd1.Parameters.Add("@phoneNumber", SqlDbType.Float);
                cmd1.Parameters["@phoneNumber"].Direction = ParameterDirection.Output;
                cmd1.Parameters.Add("@studentName", SqlDbType.VarChar, 765);
                cmd1.Parameters["@studentName"].Direction = ParameterDirection.Output;
                cmd1.Parameters.Add("@isInTime", SqlDbType.Bit);
                cmd1.Parameters["@isInTime"].Direction = ParameterDirection.Output;

                cmd1.ExecuteReader();
                con.Close();

                var attendanceResponseModel = new AttendanceResponseModel
                {
                    PhoneNumber = cmd1.Parameters["@PhoneNumber"].Value.ToString(),
                    StudentName = cmd1.Parameters["@studentName"].Value.ToString(),
                    IsInTime = Convert.ToBoolean(cmd1.Parameters["@isInTime"].Value)
                };
                return attendanceResponseModel;
            }
        }

        public static void InsertSmsResponse(SmsResponseModel smsResponseModel)
        {
            using (var con = new SqlConnection(ConnectionString))
            {
                
                //Store the SMS result in the database
                con.Open();
                var cmnd2 = new SqlCommand("InsertSMSResponse", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmnd2.Parameters.AddWithValue("@smsUrl", smsResponseModel.SmsUrl);
                cmnd2.Parameters.AddWithValue("@response", smsResponseModel.Response);
                // cmnd2.Parameters.AddWithValue("@status", smsResponseModel.Response.Substring(0, smsResponseModel.Response.IndexOf(':') - 1));
                cmnd2.Parameters.AddWithValue("@status", smsResponseModel.Status);
                cmnd2.ExecuteNonQuery();
                con.Close();
            }
        }



    }
    
}

