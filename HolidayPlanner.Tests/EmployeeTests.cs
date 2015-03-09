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
            emailServer = new Mock<EmailServer>();            
        }
        
        [TestMethod]
        public void SendHolidayRequest_WithSuccess()
        {
            //Arrange
            Employee employee = new Employee("John", "John@st.rl", EmployeeRole.Employee, new EmployeeSpecificActivity(), emailServer.Object);
            Employee manager = new Employee("Mary", "Mary@st.rl", EmployeeRole.Manager, new ManagerSpecificActivity(), emailServer.Object);
            Employee hr = new Employee("Emma", "Emma@st.rl", EmployeeRole.Hr, new HRSpecificActivity(), emailServer.Object);
 
            //Act
            employee.SendNewHolidayRequest(manager, DateTime.Now, DateTime.Now.AddDays(1));

            //Assert
            emailServer.Verify(x => x.SendEmail(It.IsAny<HolidayRequest>()), Times.Once());
            emailServer.Verify(x => x.Subscribe(It.IsAny<Employee>(), It.IsAny<Action<HolidayRequest>>()), Times.Exactly(3));
        }       

        Mock<EmailServer> emailServer;
    }
}
