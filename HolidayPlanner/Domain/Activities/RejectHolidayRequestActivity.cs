using System;
namespace HolidayPlanner.Domain.Activities
{
  public class RejectHolidayRequestActivity : IHolidayRequestActivity
  {
    public void UpdateRequest(HolidayRequest request)
    {
      request.Status = HolidayRequestStatus.Rejected;
      request.SwitchReceiverWithSender();
    }
  }
}
