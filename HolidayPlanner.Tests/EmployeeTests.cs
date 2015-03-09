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
        [TestInitialize]
        public void Setup()
        {
            emailClient = new Mock<EmailClient>();
            employee = new Employee("John", "John@st.rl", EmployeeRole.Employee, new EmployeeSpecificActivity(), emailClient.Object);
            manager = new Employee("Mary", "Mary@st.rl", EmployeeRole.Manager, new ManagerSpecificActivity(), emailClient.Object);
            hrEmployee = new Employee("Emma", "Emma@st.rl", EmployeeRole.Hr, new HRSpecificActivity(), emailClient.Object);
        }
        
        [TestMethod]
        public void SendHolidayRequest_WithSuccess()
        {           
            //Act
            employee.SendNewHolidayRequest(manager, DateTime.Now, DateTime.Now.AddDays(1));

            //Assert
            emailClient.Verify(x => x.SendEmailAsync(It.IsAny<HolidayRequest>()), Times.Once());
            emailClient.Verify(x => x.SubscribeAsync(It.IsAny<Employee>(), It.IsAny<Action<HolidayRequest>>()), Times.Exactly(3));
        }

        [TestMethod]
        public void UpdateHolidayRequest_WithSuccess()
        {
            //Arrange
            var emailClient = new EmailClient();
            employee = new Employee("John", "John@st.rl", EmployeeRole.Employee, new EmployeeSpecificActivity(), emailClient);
            manager = new Employee("Mary", "Mary@st.rl", EmployeeRole.Manager, new ManagerSpecificActivity(), emailClient);
           
            var managerChannel = string.Format("Channel: {0}", manager.Name);            

            //emailClient.Setup(e => e.SendEmail(It.IsAny<HolidayRequest>())).Callback<EmailClient>(e => e.HandleNewRequest(managerChannel, new HolidayRequest()));
           
            //Act
            employee.SendNewHolidayRequest(manager, DateTime.Now.AddDays(1), DateTime.Now);

            //Assert
            //emailClient.Verify(x => x.SendEmailAsync(It.IsAny<HolidayRequest>()), Times.Once);
            //mockActivity.Verify(a => a.ManageHolidayRequest(It.IsAny<HolidayRequest>()), Times.Never);
        }

        [TestMethod]
        public void RejectHolidayRequest_WithSuccess()
        {
            //Arrange             

            //Act
            employee.SendNewHolidayRequest(manager, DateTime.Now, DateTime.Now.AddDays(1));

            //Assert
            emailClient.Verify(x => x.SendEmailAsync(It.IsAny<HolidayRequest>()), Times.Once());
            emailClient.Verify(x => x.SubscribeAsync(It.IsAny<Employee>(), It.IsAny<Action<HolidayRequest>>()), Times.Exactly(3));
        }

        [TestMethod]
        public void ApproveHolidayRequest_WithSuccess()
        {
            //Arrange
         
            //Act
            employee.SendNewHolidayRequest(manager, DateTime.Now, DateTime.Now.AddDays(1));

            //Assert
            emailClient.Verify(x => x.SendEmailAsync(It.IsAny<HolidayRequest>()), Times.Once());
            emailClient.Verify(x => x.SubscribeAsync(It.IsAny<Employee>(), It.IsAny<Action<HolidayRequest>>()), Times.Exactly(3));
        } 

        Mock<EmailClient> emailClient;
        Employee employee;
        Employee manager;
        Employee hrEmployee;     
    }
}
