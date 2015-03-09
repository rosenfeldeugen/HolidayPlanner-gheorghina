using System.Collections.Generic;
using System.Linq;
using HolidayPlanner.Domain.Activities;
using HolidayPlanner.MessagesServer;
using System;

namespace HolidayPlanner.Domain
{
    [Serializable]
    public class Employee
    {
        private readonly IEmailClient emailClient;

        public Employee(string name, string email, EmployeeRole role, IHolidayRequestActivity requestActivity, IEmailClient emailClient)
        {
            this.emailClient = emailClient;
            Name = name;
            Email = email;
            Role = role;
            HolidayRequestActivity = requestActivity;

            emailClient.SubscribeAsync(this, OnEmailReceived);
        }

        public string Name { get; set; }

        public string Email { get; set; }

        public EmployeeRole Role { get; set; }

        public IHolidayRequestActivity HolidayRequestActivity { get; private set; }

        /// <summary>
        /// Sends the holiday request.
        /// </summary>
        /// <param name="toEmployee">To employee.</param>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        public void SendNewHolidayRequest(Employee toEmployee, DateTime fromDate, DateTime toDate)
        {
            var newHolidayRequest = new HolidayRequest
            {
                FromEmployeeName = this.Name,
                FromEmployeeEmail = this.Email,
                ToEmployeeName = toEmployee.Name,
                ToEmployeeEmail = toEmployee.Email,
                FromDate = fromDate,
                ToDate = toDate,
                Status = HolidayRequestStatus.InProgressForApproval
            };
            emailClient.SendEmailAsync(newHolidayRequest);
        }

        internal void OnEmailReceived(HolidayRequest newRequest)
        {
            HolidayRequestActivity.ManageHolidayRequest(newRequest);
            emailClient.SendEmailAsync(newRequest);
        }
    }
}
