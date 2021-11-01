using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace Core.Utils
{
    public abstract class Helper
    {
        public static T Deserialize<T>(string jsonString, Action<Exception> logAction = null)
        {
            T t = default(T);

            try
            {
                t = JsonSerializer.Deserialize<T>(jsonString);
            }
            catch (Exception ex)
            {
                logAction?.Invoke(ex);
            }

            return t;
        }

        public static string Serialize(object value, Action<Exception> logAction = null)
        {
            string serializedObject = null;

            try
            {
                serializedObject = JsonSerializer.Serialize(value);
            }
            catch (Exception ex)
            {
                logAction?.Invoke(ex);
            }

            return serializedObject;
        }

        public static string SplitThenTakeByIndex(string word, int index, char seperator = '.')
        {
            var splittedValues = word.Split(seperator);

            if (splittedValues.Length == 0)
                return null;

            if (index < 0)
            {
                return splittedValues.TakeLast(Math.Abs(index)).FirstOrDefault();
            }

            return splittedValues[index];
        }

        public static byte[] StringToByteArray(string value, Encoding encoding = null, Action<Exception> logAction = null)
        {
            byte[] array = null;

            try
            {
                encoding = encoding ?? Encoding.UTF8;

                array = encoding.GetBytes(value);
            }
            catch (Exception ex)
            {
                logAction?.Invoke(ex);
            }

            return array;
        }
    }
}
