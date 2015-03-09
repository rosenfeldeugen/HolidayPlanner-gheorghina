using System;

namespace HolidayPlanner.Domain.Activities
{
    public class ManagerSpecificActivity : IHolidayRequestActivity
    {
        public void ManageHolidayRequest(HolidayRequest request)
        {
            if (request.IsValid())
            {
                request.Status = HolidayRequestStatus.Approved;
                return;
            }

            request.Status = HolidayRequestStatus.Rejected;
        }
    }
}
