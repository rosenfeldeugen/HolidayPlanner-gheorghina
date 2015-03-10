
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

        public void UpdateSender(string name, string email)
        {
            this.FromEmployeeName = name;
            this.FromEmployeeEmail = email;
        }

        public void UpdateRecipient(string name, string email)
        {
            this.ToEmployeeName = name;
            this.ToEmployeeEmail = email;
        }

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
