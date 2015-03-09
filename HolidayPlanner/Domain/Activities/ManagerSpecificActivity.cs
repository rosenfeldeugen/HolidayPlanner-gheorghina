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
                request.UpdateSender(request.ToEmployeeName, request.ToEmployeeEmail);
                request.UpdateRecipient(Properties.Settings.Default.HrName, Properties.Settings.Default.HrEmail);               
                return;
            }

            request.SwitchReceiverWithSender();
            request.Status = HolidayRequestStatus.Rejected;
        }
    }
}
