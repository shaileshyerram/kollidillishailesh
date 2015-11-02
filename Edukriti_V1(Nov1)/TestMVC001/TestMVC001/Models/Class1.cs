using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestMVC001.Models
{
    public class AttendanceData
    {
        public int AttendanceId { get; set; }
        public string OrgId { get; set; }
        public string MachineId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public DateTime DateOfTransaction { get; set; }
    }
}