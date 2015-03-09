
using System;

namespace HolidayPlanner.Domain
{
    [Serializable]
    public class HolidayRequest
    {
        public Employee FromEmployee { get; set; }

        public Employee ToEmployee { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public HolidayRequestStatus Status { get; set; }

        public bool IsValid()
        {
            return FromDate < ToDate; //And others
        }

    }
}
