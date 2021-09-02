using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Manager.ValueHistory
{
    public class ValueHistoryRecord
    {
        public Guid Id { get; set; }

        public DateTime RecordDate { get; set; }

        public Type Type { get; set; }

        public object Value { get; set; }

        public string PropertyName { get; set; }

        public string RecorderInfo { get; set; }

        public ValueHistoryRecord(string propertyName, object value, string recorderInfo = null)
        {
            Id = Guid.NewGuid();

            RecordDate = DateTime.Now;

            Type = value.GetType();

            Value = value;

            PropertyName = propertyName;

            RecorderInfo = recorderInfo;
        }

        public T GetValueAs<T>()
        {
            try
            {
                return (T)Value;
            }
            catch (InvalidCastException ex)
            {
                throw ex;
            }
        }
    }
}
