using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Manager.ValueHistory
{
    public class ValueHistoryManager
    {
        public List<ValueHistoryRecord> Records { get; set; }

        public List<ValueHistorySetting> Settings { get; set; }

        public int ChangingCount { get; set; }

        public ValueHistoryManager()
        {
            Records = new List<ValueHistoryRecord>();

            Settings = new List<ValueHistorySetting>();
        }

        public ValueHistoryManager AddSettings(object obj, bool addCurrentValues = true, int defaultAmountForEachProperty = 2)
        {
            var properties = obj.GetType().GetProperties().Where(p => p.CanWrite);

            foreach (var property in properties)
            {
                AddSetting(new ValueHistorySetting(property.Name, defaultAmountForEachProperty));

                if (addCurrentValues)
                {
                    var propertyInfo = obj.GetType().GetProperty(property.Name);

                    Records.Add(new ValueHistoryRecord(property.Name, propertyInfo.GetValue(obj)));
                }
            }

            return this;
        }

        public ValueHistoryManager AddSetting(ValueHistorySetting setting)
        {
            if (!Settings.Any(s => s.PropertyName == setting.PropertyName))
                Settings.Add(setting);
            else
            {
                var relatedSetting = Settings.FirstOrDefault(s => s.PropertyName == setting.PropertyName);

                relatedSetting.Update(setting.Amount, setting.ControlActionWhetherValueIsNew);
            }

            return this;
        }

        public void Update(string propertyName = null)
        {
            ValueHistorySetting relatedSetting = null;

            if (!string.IsNullOrEmpty(propertyName))
                relatedSetting = Settings.FirstOrDefault(s => s.PropertyName == propertyName);

            foreach (var setting in (relatedSetting != null ? new List<ValueHistorySetting>() { relatedSetting } : Settings))
            {
                var predicate = new Func<ValueHistoryRecord, bool>((r) => r.PropertyName == setting.PropertyName);

                while (Records.Count(predicate) > setting.Amount)
                {
                    Records.Remove(Records.Where(predicate)
                                        .OrderBy(r => r.RecordDate)
                                        .First());
                }
            }
        }

        public IEnumerable<ValueHistoryRecord> GetRecords(Func<ValueHistoryRecord, bool> predicate, int? toTake = null)
        {
            Update();

            toTake = toTake ?? Records.Count;

            return Records.Where(predicate)
                            .OrderByDescending(r => r.RecordDate)
                            .Take(toTake.Value);
        }

        public IEnumerable<ValueHistoryRecord> GetRecordsByPropertyName(string propertyName, int? toTake = null)
        {
            return GetRecords(r => r.PropertyName == propertyName, toTake);
        }

        private bool HasChangedFor(ValueHistoryRecord record)
        {
            if (IsRecordHasNewValue(record))
            {
                if (Records.Count > 0 && Records.Any(r => r.PropertyName == record.PropertyName))
                {
                    var maxRecordDate = Records.Where(r => r.PropertyName == record.PropertyName).Max(r => r.RecordDate);

                    if (maxRecordDate >= record.RecordDate)
                        record.RecordDate = maxRecordDate.AddTicks(1);
                }

                Records.Add(record);

                Update(record.PropertyName);

                return true;
            }

            return false;
        }


        public bool HasChangedFor(object obj)
        {
            //return Settings.Select(s => new
            //{
            //    Setting = s,
            //    PropertyInfo = obj.GetType().GetProperty(s.PropertyName)
            //}).Any(o => HasChangedFor(new ValueHistoryRecord(o.Setting.PropertyName, o.PropertyInfo.GetValue(obj))));

            var isChanged = false;

            foreach (var setting in Settings)
            {
                var propertyInfo = obj.GetType().GetProperty(setting.PropertyName);

                if (propertyInfo == null)
                    continue;

                isChanged = HasChangedFor(new ValueHistoryRecord(setting.PropertyName, propertyInfo.GetValue(obj)));

                if (isChanged)
                {
                    ChangingCount++;

                    break;
                }
            }

            return isChanged;
        }

        public bool IsRecordHasNewValue(ValueHistoryRecord record)
        {
            var hasNewValue = false;

            if (Records.Any(r => r.PropertyName == record.PropertyName))
            {
                var lastRecord = Records.Where(r => r.PropertyName == record.PropertyName)
                                        .OrderByDescending(r => r.RecordDate)
                                        .First();

                var setting = Settings.FirstOrDefault(s => s.PropertyName == record.PropertyName);

                if (setting.ControlActionWhetherValueIsNew != null)
                    hasNewValue = setting.ControlActionWhetherValueIsNew(lastRecord.Value, record.Value);
                else
                    hasNewValue = true;
            }
            else
                hasNewValue = true;

            return hasNewValue;
        }
    }
}
