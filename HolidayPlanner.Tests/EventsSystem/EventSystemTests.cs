
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HolidayPlanner.EventsSystem;

namespace HolidayPlanner.Tests
{
    /// <summary>
    /// Summary description for CacheManagerTests
    /// </summary>
    [TestClass]
    public class EventSystemTests
    {
        [TestInitialize]
        public void Setup()
        {
            eventSystem = new EventSystem();
        }

        [TestCleanup]
        public void Cleanup()
        {
            eventSystem.UnsubscribeAllAsync();
        }

        [TestMethod]
        public void EventSystem_SubscribeAsync_WithSuccess()
        {
            //Arrange
            var expected = new MockEntity(TestKey, TestValue);

            //Act
            var result = new MockEntity("0", "none");

            var subscribeAsyncTask = Task.Factory.StartNew(() => eventSystem.SubscribeAsync<MockEntity>(TestKey, ChannelPattern.Literal, (channel, value) => { result = value; }));
            subscribeAsyncTask.Wait();
            
            var publishAsyncTask = Task.Factory.StartNew(async () => await eventSystem.PublishAsync(TestKey, ChannelPattern.Literal, expected));
            publishAsyncTask.Wait();

            Thread.Sleep(5000);

            //Assert
            Assert.AreEqual(expected, result);  
        }
               
        private IEventSystem eventSystem;
        private const string TestKey = "TestKey";
        private const string TestValue = "TestValue";
        private const string SecondTestValue = "SecondTestValue";
        private const string ValueWasSetMessageFormat = "value was set {0} times";
    }
}
