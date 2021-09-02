using Core.Manager.ValueHistory;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace XUnitTest.Manager.ValueHistory
{
    public class ValueHistoryManagerTest
    {
        public class TestObject
        {
            public string Name { get; set; }
        }

        [Fact]
        public void ObjectHasChanged()
        {
            var to = new TestObject();

            var valueHistoryManager = new ValueHistoryManager();

            valueHistoryManager.AddSettings(to);

            to.Name = "Name1";

            Assert.True(valueHistoryManager.HasChangedFor(to));
        }
    }
}
