using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HolidayPlanner.MessagesServer;
using HolidayPlanner.Domain;
using HolidayPlanner.EventsSystem;
using HolidayPlanner.Domain.Activities;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace HolidayPlanner.Tests
{
    [TestClass]
    public class EmployeeTests
    {        
        [TestMethod]
        public void SendHolidayRequest_WithSuccess()
        {           
            //Arrange
            var emailClient = new Mock<EmailClient>();
            var employee = new Employee("John", "John@st.rl", EmployeeRole.Employee, employeeSupportedActivities, emailClient.Object);
            var manager = new Employee("Mary", "Mary@st.rl", EmployeeRole.Manager, managerSupportedActivities, emailClient.Object);

            //Act
            var publishAsyncTask = Task.Factory.StartNew(async () => await employee.SendNewHolidayRequest(manager, DateTime.Now, DateTime.Now.AddDays(1)));
            Thread.Sleep(5000);

            //Assert
            emailClient.Verify(x => x.SendEmailAsync(It.IsAny<HolidayRequest>()), Times.Once());
            emailClient.Verify(x => x.SubscribeAsync(It.IsAny<Employee>(), It.IsAny<Action<HolidayRequest>>()), Times.Exactly(2));
        }

        [TestMethod]
        public void UpdateHolidayRequest_PassInvalidRequest_ItWillBeUpdatedAndPassedToHrInTheEnd()
        {
            //Arrange
            var emailClient = new EmailClient();
            var employee = new Employee("John", "John@st.rl", EmployeeRole.Employee, employeeSupportedActivities, emailClient);
            var manager = new Employee("Mary", "Mary@st.rl", EmployeeRole.Manager, managerSupportedActivities, emailClient);
                 
            //Act
            var publishAsyncTask = Task.Factory.StartNew(async () => await employee.SendNewHolidayRequest(manager, DateTime.Now.AddDays(1), DateTime.Now));
            Thread.Sleep(5000);
            
            publishAsyncTask = Task.Factory.StartNew(async () => await manager.UpdateHolidayRequest(manager.ReceivedHolidayRequests.First(), new RejectHolidayRequestActivity()));
            Thread.Sleep(5000);

            publishAsyncTask = Task.Factory.StartNew(async () => await employee.UpdateHolidayRequest(employee.ReceivedHolidayRequests.First(), new UpdateHolidayRequestActivity()));
            Thread.Sleep(5000);

            //Assert
            //that the thread does not get stuck in an infinite loop because of back and forth messaging
            Assert.AreEqual(0, employee.ReceivedHolidayRequests.Count());
        }

        private List<IHolidayRequestActivity> employeeSupportedActivities = new List<IHolidayRequestActivity>{
            new UpdateHolidayRequestActivity()
        };

        private List<IHolidayRequestActivity> managerSupportedActivities = new List<IHolidayRequestActivity>{
            new UpdateHolidayRequestActivity(),
            new RejectHolidayRequestActivity(),
            new ApproveHolidayRequestActivity()
        };
    }
}
