
using System;

namespace HolidayPlanner.Domain
{
    [Serializable]
    public class HolidayRequest
    {
        public string FromEmployeeName { get; set; }

        public string FromEmployeeEmail { get; set; }

        public string ToEmployeeName { get; set; }

        public string ToEmployeeEmail { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public HolidayRequestStatus Status { get; set; }

        public bool IsValid()
        {
            return FromDate < ToDate; //And others
        }

    }
}
