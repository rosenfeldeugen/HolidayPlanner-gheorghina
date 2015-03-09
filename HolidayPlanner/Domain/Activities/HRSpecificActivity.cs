using System;

namespace HolidayPlanner.Domain.Activities
{
    public class HRSpecificActivity : IHolidayRequestActivity
    {
        public void ManageHolidayRequest(HolidayRequest request)
        {
            request.Status = HolidayRequestStatus.Processed;
        }
    }
}
