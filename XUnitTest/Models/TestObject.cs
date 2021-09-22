using System;
using System.Collections.Generic;
using System.Text;

namespace XUnitTest.Models
{
    public class TestObject
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public TestObject()
        {
            Id = Guid.NewGuid();
        }
    }
}
