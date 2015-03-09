using System;

namespace HolidayPlanner.Domain.Activities
{
    public class EmployeeSpecificActivity : IHolidayRequestActivity
    {
        public void ManageHolidayRequest(HolidayRequest request)
        {
            request.Status = HolidayRequestStatus.InProgressForApproval;
        }
    }
}
