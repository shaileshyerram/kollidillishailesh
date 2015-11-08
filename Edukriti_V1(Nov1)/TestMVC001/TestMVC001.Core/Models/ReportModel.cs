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
}
