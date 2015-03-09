
using HolidayPlanner.EventsSystem;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace HolidayPlanner.Tests
{
    [TestClass]
    public class BinarySerializerTests
    {
        [TestMethod]
        public void BinarySerializer_Serialize_WithSuccess()
        {
            //Arrange
            var mockEntity = new MockEntity("1", "name");
            var serializer = new BinarySerializer();
            const int expectedStreamLength = 207;

            //Act
            var result = serializer.Serialize(mockEntity);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedStreamLength, result.Length);
        }
        		

        [TestMethod]
        public void BinarySerializer_Deserialize_WithSuccess()
        {
            const string testString = "\0\0\0\0????\0\0\0\0\0\0\0\f\0\0\0KHolidayPlanner.Tests, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\0\0\0HolidayPlanner.Tests.MockEntity\0\0\0<Key>k__BackingField<Value>k__BackingField\0\0\0\0\0\01\0\0\0name\v";
            var testStream = System.Text.Encoding.UTF8.GetBytes(testString);

            var expectedEntity = new MockEntity("1", "name");
            var serializer = new BinarySerializer();

            //Act
            var result = serializer.Deserialize<MockEntity>(testStream);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(MockEntity));
            Assert.AreEqual(expectedEntity, result);

        }        
    }

    [Serializable]
    public class MockEntity
    {
        public MockEntity(string key, string value)
        {
            Key = key;
            Value = value;
        }

        public string Key { get; private set; }

        public string Value { get; private set; }

        public override bool Equals(object obj)
        {
            return 
                    obj != null &&
                    obj.GetType() == typeof(MockEntity) &&
                    this.Key == (obj as MockEntity).Key &&
                    this.Value == (obj as MockEntity).Value;
        }       
    }
}
