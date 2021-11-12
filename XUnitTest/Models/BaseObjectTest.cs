using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Model;
using Xunit;

namespace XUnitTest.Models
{
    public class BaseObjectTest
    {
        [Fact]
        public void SetFunctionTest()
        {
            var to = new TestObject();

            var name = "SampleName";

            to.Set(t => t.Name, name); 

            Assert.Equal(to.Name, name);
        }
    }
}
