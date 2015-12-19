using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using TestMVC001.Core.Models;

namespace TestMVC001.Core.Services
{
    public class NotificationService
    {
        static readonly string ConnectionString = WebConfigurationManager.ConnectionStrings["myConnectionString"].ToString();
        public static ReportResponseModel GetSMSTree(string orgId)
        {
            DataSet ds = new DataSet("TimeRanges");
            var reportResponseModel = new ReportResponseModel();
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                var sqlComm = new SqlCommand("GetSMSTree", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                sqlComm.Parameters.AddWithValue("@orgId", orgId);
                SqlDataAdapter da = new SqlDataAdapter { SelectCommand = sqlComm };
                da.Fill(ds);
                //sqlComm.ExecuteNonQuery();
                SqlDataReader dr = sqlComm.ExecuteReader();
                var parentsList = new List<Parents>();
                while (dr.Read())
                {
                    /*parentsList.Add(new Entity
                    {
                        Parents = dr["txtEntity"].ToString(),
                        Parent = 
                    });*/
                }
                con.Close();
                
            }
            return reportResponseModel;
        }
    }

    
    public class Parents
    {
        public Entity Entity;
        public Entity Parent;
    }
    public enum Entity
    {
        Student,
        TeachingStaff,
        NonTeachingStaff
    }
}
