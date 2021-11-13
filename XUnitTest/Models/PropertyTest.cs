using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Model;
using Xunit;

namespace XUnitTest.Models
{
    public class PropertyTest
    {
        [Fact]
        public void NameAndPropertyValueTest()
        {
            var to = new TestObject();

            to.SetName("TestObject1");

            var properties = Property.LoadPropertiesFromObject(to);

            var property = properties.Where(p=>p.Name == "Name").FirstOrDefault();

            Assert.True(property.Value.ToString() == "TestObject1");
        }
    }
}
