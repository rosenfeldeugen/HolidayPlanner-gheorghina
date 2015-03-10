using System;

namespace HolidayPlanner.Domain.Activities
{
    public class ProcessHolidayRequestActivity : IHolidayRequestActivity
    {
        public void UpdateRequest(HolidayRequest request)
        {    
            request.Status = HolidayRequestStatus.Processed;
        }
    }
}
