using System;

namespace TestMVC001.Core.Models
{
    public class AttendanceRequestModel
    {
        public string OrgId;
        public string MachineId;
        public string RfId;
        public DateTime DtAttendance;
    }

    public class AttendanceResponseModel
    {
        public string PhoneNumber;
        public string StudentName;
        public bool IsInTime;
    }
}
