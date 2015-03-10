using System;

namespace HolidayPlanner.Domain.Activities
{
    public class UpdateHolidayRequestActivity : IHolidayRequestActivity
    {
        public void UpdateRequest(HolidayRequest request)
        {
            request.SwitchReceiverWithSender();
            request.Status = HolidayRequestStatus.InProgressForApproval;
        }       
    }
}
