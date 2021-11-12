using System;
using System.Collections.Generic;
using System.Text;
using Core.Model;

namespace XUnitTest.Models
{
    public class TestObject : BaseObject
    {
        public TestObject()
        {
            SetId(Guid.NewGuid());
        }
    }
}
