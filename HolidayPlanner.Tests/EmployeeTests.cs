using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HolidayPlanner.MessagesServer;
using HolidayPlanner.Domain;
using HolidayPlanner.EventsSystem;
using HolidayPlanner.Domain.Activities;
using Moq;

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
            var employee = new Employee("John", "John@st.rl", EmployeeRole.Employee, new EmployeeSpecificActivity(), emailClient.Object);
            var manager = new Employee("Mary", "Mary@st.rl", EmployeeRole.Manager, new ManagerSpecificActivity(), emailClient.Object);

            //Act
            employee.SendNewHolidayRequest(manager, DateTime.Now, DateTime.Now.AddDays(1));

            //Assert
            emailClient.Verify(x => x.SendEmailAsync(It.IsAny<HolidayRequest>()), Times.Once());
            emailClient.Verify(x => x.SubscribeAsync(It.IsAny<Employee>(), It.IsAny<Action<HolidayRequest>>()), Times.Exactly(3));
        }

        [TestMethod]
        public void UpdateHolidayRequest_PassInvalidRequest_ItWillBeUpdatedAndPassedToHrInTheEnd()
        {
            //Arrange
            var emailClient = new EmailClient();
            var employee = new Employee("John", "John@st.rl", EmployeeRole.Employee, new EmployeeSpecificActivity(), emailClient);
            var manager = new Employee("Mary", "Mary@st.rl", EmployeeRole.Manager, new ManagerSpecificActivity(), emailClient);
                 
            //Act
            employee.SendNewHolidayRequest(manager, DateTime.Now.AddDays(1), DateTime.Now);

            //Assert
            //that the thread does not get stuck in an infinite loop because of back and forth messaging

        }           
    }
}
