using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Model;
using Xunit;

namespace XUnitTest.Models
{
    public class ClassInfoTest
    {
        [Fact]
        public void CreateFromObjectTest()
        {
            var to = new TestObject();

            to.SetName("TestObject1");

            var classInfo = ClassInfo.CreateFromObject(to);

            Assert.True(classInfo.Name == to.GetType().Name);

            var property = classInfo.GetPropertyByName("Name");

            Assert.True(property.Value.ToString()== "TestObject1");
        }
    }
}
