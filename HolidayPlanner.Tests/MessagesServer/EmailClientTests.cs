using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HolidayPlanner.MessagesServer;
using HolidayPlanner.Domain;
using HolidayPlanner.EventsSystem;
using HolidayPlanner.Domain.Activities;
using System.Threading.Tasks;
using System.Threading;

namespace HolidayPlanner.Tests
{
    [TestClass]
    public class EmailClientTests
    {
        [TestInitialize]
        public void Setup()
        {
            eventsystem = new EventSystem();
            emailClient = new EmailClient(eventsystem);
            employee = new Employee("John", "John@st.rl", EmployeeRole.Employee, new EmployeeSpecificActivity(), emailClient);
            manager = new Employee("Mary", "Mary@st.rl", EmployeeRole.Manager, new ManagerSpecificActivity(), emailClient);
            initialHolidayRequest = new HolidayRequest { 
                ToEmployeeName = manager.Name, 
                ToEmployeeEmail = manager.Email,
                FromEmployeeName = employee.Name,
                FromEmployeeEmail = employee.Email, 
                FromDate = DateTime.Now, ToDate = DateTime.Now.AddDays(1), Status = HolidayRequestStatus.InProgressForApproval };

        }

        [TestMethod]
        public void SendEmail_WithSuccess()
        {
            //Arrange 
            HolidayRequest receivedHolidayRequest = null;

            Action<string, HolidayRequest> handleRequest = (channel, holidayRequest) =>
            {
                receivedHolidayRequest = holidayRequest;
            };

            var managerChannel = string.Format("Channel: {0}", manager.Name);            

            //Act

            var subscribeAsyncTask = Task.Factory.StartNew(async () => await eventsystem.SubscribeAsync<HolidayRequest>(managerChannel, ChannelPattern.Literal, handleRequest));
            subscribeAsyncTask.Wait();

            var publishAsyncTask = Task.Factory.StartNew(async () => await emailClient.SendEmailAsync(initialHolidayRequest));
            publishAsyncTask.Wait();

            Thread.Sleep(5000);
                       
            //Assert
            Assert.IsNotNull(receivedHolidayRequest);
        }

        EventSystem eventsystem;
        EmailClient emailClient;
        Employee employee;
        Employee manager;
        HolidayRequest initialHolidayRequest;
    }
}
