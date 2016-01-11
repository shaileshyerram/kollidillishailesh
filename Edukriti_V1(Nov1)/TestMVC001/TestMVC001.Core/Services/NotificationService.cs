using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Configuration;
using TestMVC001.Core.Models;

namespace TestMVC001.Core.Services
{
    public class NotificationService
    {
        static readonly string ConnectionString = WebConfigurationManager.ConnectionStrings["myConnectionString"].ToString();
        public static List<EntityItem> GetSMSTree(string orgId)
        {
            var ds = new DataSet("TimeRanges");
            var items = new List<EntityItem>();
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                var sqlComm = new SqlCommand("GetSMSTree", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                sqlComm.Parameters.AddWithValue("@orgId", orgId);
                var da = new SqlDataAdapter { SelectCommand = sqlComm };
                da.Fill(ds);
                SqlDataReader dr = sqlComm.ExecuteReader();
                
                while (dr.Read())
                {
                    if (dr["txtEntity"] != null)
                    {
                        var item = new EntityItem
                        {
                            Entity = dr["txtEntity"].ToString(),
                            Id = dr["id"].ToString()
                        };
                        if (dr["txtParent"] != null)
                            item.Parent = dr["txtParent"].ToString();
                        items.Add(item);
                    }
                }
                con.Close();
            }
            return items;
        }
        public static List<string> GetPhoneNumbers(List<string> groupIdsList, string orgId)
        {
            var phoneNumber = new List<string>();
            var ds = new DataSet("TimeRanges");
            var items = new List<EntityItem>();
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                var sqlComm = new SqlCommand("GetPhoneNumbers", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                sqlComm.Parameters.AddWithValue("@groupList", string.Join(",", groupIdsList.Select(x => x.ToString()).ToArray()));
                sqlComm.Parameters.AddWithValue("@orgId", orgId);
                var da = new SqlDataAdapter { SelectCommand = sqlComm };
                da.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow dataRow in ds.Tables[0].Rows)
                    {
                        foreach (var cells in dataRow.ItemArray)
                        {
                            phoneNumber.Add(cells.ToString());
                        }
                    }
                }
            }
            return phoneNumber;
        }
    }

    public class EntityItem
    {
        public string Id;
        public string Entity;
        public string Parent;
    }
}
