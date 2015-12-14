using System;
using System.Collections.Generic;
using System.Data;
using Newtonsoft.Json;

namespace TestMVC001.Core.Models
{
    public class ReportRequestModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("studentclass")]
        public string StudentClass { get; set; }

        [JsonProperty("section")]
        public string Section { get; set; }

        [JsonProperty("dtfrom")]
        public DateTime DtFrom { get; set; }

        [JsonProperty("dtto")]
        public DateTime DtTo { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }
    }
    
    public class ReportResponseModel
    {
        public List<Row> Rows;
        public List<string> Columns;
    }

    public class Row
    {
        public List<string> RowCells;
    }

    public class ReportResponseModelWithContacts
    {
        public string Name;
        public string Class;
        public string Section;
        public string Gender;
        public string RFID;
        public string AttendanceDate;
        public string InTime;
        public string OutTime;
        public string Duration;
        public string UserName;
        public string PhoneNumber;
    }

}
