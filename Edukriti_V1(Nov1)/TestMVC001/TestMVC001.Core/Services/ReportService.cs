using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using TestMVC001.Core.Models;

namespace TestMVC001.Core.Services
{
    public class ReportService
    {
        static readonly string ConnectionString = WebConfigurationManager.ConnectionStrings["myConnectionString"].ToString();
        public static ReportResponseModel GetAttendanceReport(ReportRequestModel requestModel)
        {
            DataSet ds = new DataSet("TimeRanges");
            var reportResponseModel = new ReportResponseModel();
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                var sqlComm = new SqlCommand("GetAttendanceReport", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                sqlComm.Parameters.AddWithValue("@name", requestModel.Name);
                sqlComm.Parameters.AddWithValue("@class", requestModel.StudentClass);
                sqlComm.Parameters.AddWithValue("@section", requestModel.Section);
                sqlComm.Parameters.AddWithValue("@fromDate", requestModel.DtFrom);
                sqlComm.Parameters.AddWithValue("@toDate", requestModel.DtTo);
                sqlComm.Parameters.AddWithValue("@category", requestModel.Category); 
                SqlDataAdapter da = new SqlDataAdapter {SelectCommand = sqlComm};
                da.Fill(ds);
                //sqlComm.ExecuteReader();
                sqlComm.ExecuteNonQuery();
                con.Close();
                if (ds.Tables.Count > 0)
                {
                    reportResponseModel.Columns = new List<string>();
                    foreach (DataColumn column in ds.Tables[0].Columns)
                    {
                        reportResponseModel.Columns.Add(column.ColumnName);
                    }
                    reportResponseModel.Rows = new List<Row>();
                    foreach (DataRow dataRow in ds.Tables[0].Rows)
                    {
                        var row = new Row();
                        row.RowCells = new List<string>();
                        foreach (var cells in dataRow.ItemArray)
                        {
                            row.RowCells.Add(cells.ToString());
                        }
                        reportResponseModel.Rows.Add(row);
                    }
                }
            }
            return reportResponseModel;
        }
    }
}
