using System.Collections.Generic;
using System.Linq;
using HolidayPlanner.Domain.Activities;
using HolidayPlanner.MessagesServer;
using System;

namespace HolidayPlanner.Domain
{
    public class Employee
    {
        private readonly IEmailClient emailServer;

        public Employee(string name, string email, EmployeeRole role, IHolidayRequestActivity requestActivity, IEmailClient emailServer)
        {
            this.emailServer = emailServer;
            Name = name;
            Email = email;
            Role = role;
            HolidayRequestActivity = requestActivity;

            emailServer.Subscribe(this, OnEmailReceived);
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
                FromEmployee = this,
                ToEmployee = toEmployee,
                FromDate = fromDate,
                ToDate = toDate,
                Status = HolidayRequestStatus.InProgressForApproval
            };
            emailServer.SendEmail(newHolidayRequest);
        }

        private void OnEmailReceived(HolidayRequest newRequest)
        {
            HolidayRequestActivity.ManageHolidayRequest(newRequest);
            emailServer.SendEmail(newRequest);
        }
    }
}
