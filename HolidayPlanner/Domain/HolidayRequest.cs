
using System;

namespace HolidayPlanner.Domain
{
    [Serializable]
    public class HolidayRequest
    {
        public string FromEmployeeName { get; set; }

        public string FromEmployeeEmail { get; set; }

        public string ToEmployeeName { get; set; }

        public string ToEmployeeEmail { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public HolidayRequestStatus Status { get; set; }

        //CR: Don't see the reason for this function as long as FromEmployeeName and FromEmployeeEmail are public
        public void UpdateSender(string name, string email)
        {
            //CR: this. is not necessary here
            this.FromEmployeeName = name;
            this.FromEmployeeEmail = email;
        }

        //CR: idem
        public void UpdateRecipient(string name, string email)
        {
            this.ToEmployeeName = name;
            this.ToEmployeeEmail = email;
        }

        //CR: I don't know if this should belong to HolidayRequest.
        //from HolidayRequest point of view, this makes no sense
        public void SwitchReceiverWithSender()
        {
            var fromName = this.FromEmployeeName;
            var fromEmail = this.FromEmployeeEmail;

            this.FromEmployeeEmail = this.ToEmployeeEmail;
            this.FromEmployeeName = this.ToEmployeeName;
            
            this.ToEmployeeEmail = fromName;
            this.ToEmployeeName = fromEmail;
        }

    }
}
