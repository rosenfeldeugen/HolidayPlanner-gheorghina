using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HolidayPlanner.Domain
{
    [Serializable]
    public enum HolidayRequestStatus
    {
        InProgressForApproval,
        Rejected,
        Approved,
        Processed
    }
}
