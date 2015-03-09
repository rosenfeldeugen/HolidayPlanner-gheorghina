using System;

namespace HolidayPlanner.Domain.Activities
{
    public class EmployeeSpecificActivity : IHolidayRequestActivity
    {
        public void ManageHolidayRequest(HolidayRequest request)
        {
            UpdateRequest(request);
            request.Status = HolidayRequestStatus.InProgressForApproval;
        }

        private void UpdateRequest(HolidayRequest request)
        {
            //Specific Update Logic that will come from a user interaction
            if (!request.IsValid())
            {
                request.FromDate = request.ToDate.AddDays(-1);
            }

            request.SwitchReceiverWithSender();
        }
    }
}
