using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Manager.ValueHistory
{
    public class ValueHistorySetting
    {
        public Type Type { get; set; }

        public string PropertyName { get; private set; }

        public int Amount { get; private set; }

        public readonly int DefaultAmount = 2;

        public Func<object, object, bool> ControlActionWhetherValueIsNew { get; set; }

        public ValueHistorySetting(string propertyName, int amount = 2, Func<object, object, bool> action = null)
        {
            PropertyName = propertyName;

            Update(amount, action);
        }

        public ValueHistorySetting Update(int amount, Func<object, object, bool> action = null)
        {
            Amount = amount < DefaultAmount ? DefaultAmount : amount;

            ControlActionWhetherValueIsNew = action ??
                new Func<object, object, bool>((previousValue, currentValue) =>
                {
                    return !previousValue.Equals(currentValue);
                });

            return this;
        }
    }
}
