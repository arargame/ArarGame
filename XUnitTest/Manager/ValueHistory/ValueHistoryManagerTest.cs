using Core.Manager.ValueHistory;
using Xunit;
using XUnitTest.Models;

namespace XUnitTest.Manager.ValueHistory
{
    public class ValueHistoryManagerTest
    {
        [Fact]
        public void ValueHistoryManager_HasChangedForObject()
        {
            var to = new TestObject()
            {
                Name = "Name0"
            };

            var valueHistoryManager = new ValueHistoryManager();

            valueHistoryManager.AddSettings(to);

            to.Name = "Name1";

            var isChanged = valueHistoryManager.HasChangedFor(to);

            Assert.True(isChanged);

            to.Name = "Name2";

            isChanged = valueHistoryManager.HasChangedFor(to);

            Assert.True(isChanged);

            isChanged = valueHistoryManager.HasChangedFor(to);

            Assert.True(!isChanged);
        }
    }
}
