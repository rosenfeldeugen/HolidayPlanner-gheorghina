using System;

namespace HolidayPlanner.Domain.Activities
{
    public class ApproveHolidayRequestActivity : IHolidayRequestActivity
    {
        public void UpdateRequest(HolidayRequest request)
        {  
            request.Status = HolidayRequestStatus.Approved;
            request.UpdateSender(request.ToEmployeeName, request.ToEmployeeEmail);
            request.UpdateRecipient(Properties.Settings.Default.HrName, Properties.Settings.Default.HrEmail);               
        }
    }
}
