using System.Collections.Generic;
using System.Linq;
using HolidayPlanner.Domain.Activities;
using HolidayPlanner.MessagesServer;
using System;
using HolidayPlanner.Domain.Exceptions;
using System.Threading.Tasks;

namespace HolidayPlanner.Domain
{
    [Serializable]
    public class Employee
    {
        private readonly IEmailClient emailClient;        

        public Employee(string name, string email, EmployeeRole role, List<IHolidayRequestActivity> supportedActivities, IEmailClient emailClient)
        {
            this.emailClient = emailClient;
            Name = name;
            Email = email;
            Role = role;
            SupportedHolidayRequestActivities = supportedActivities;
            ReceivedHolidayRequests = new List<HolidayRequest>();

            emailClient.SubscribeAsync(this, OnEmailReceived);
        }

        public string Name { get; set; }

        public string Email { get; set; }

        public EmployeeRole Role { get; set; }

        public List<IHolidayRequestActivity> SupportedHolidayRequestActivities { get; private set; }

        public List<HolidayRequest> ReceivedHolidayRequests{get; private set;}

        /// <summary>
        /// Sends the holiday request.
        /// </summary>
        /// <param name="toEmployee">To employee.</param>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        public async Task SendNewHolidayRequest(Employee toEmployee, DateTime fromDate, DateTime toDate)
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
            await emailClient.SendEmailAsync(newHolidayRequest);
        }

        public async Task UpdateHolidayRequest(HolidayRequest request, IHolidayRequestActivity activity) 
        {
            if (!SupportedHolidayRequestActivities.Contains(activity) || !ReceivedHolidayRequests.Contains(request))
            {
                throw new UnsupportedActionException();
            }

            activity.UpdateRequest(request);
            await emailClient.SendEmailAsync(request);
            
        }

        //CR: this can be private
        internal void OnEmailReceived(HolidayRequest newRequest)
        {
            ReceivedHolidayRequests.Add(newRequest);
        }

    }
}
