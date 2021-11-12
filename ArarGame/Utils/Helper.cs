using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace Core.Utils
{
    public abstract class Helper
    {
        public static T Deserialize<T>(string jsonString, Action<Exception> logAction = null)
        {
            var t = default(T);

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

        public static PropertyInfo? GetPropertyOf(Type type,string propertyName)
        {
            return type.GetProperty(propertyName);
        }

        public static PropertyInfo? GetPropertyOf<T>(string propertyName) where T : class
        {
            return typeof(T).GetProperty(propertyName);
        }

        public static PropertyInfo[] GetPropertiesOf(Type type,BindingFlags bindingFlags = BindingFlags.Default, Action<Exception> logAction = null)
        {
            try
            {
                return type.GetProperties(bindingFlags);
            }
            catch (Exception ex)
            {
                logAction?.Invoke(ex);
            }

            return null;
        }


        public static PropertyInfo[] GetPropertiesOf<T>(BindingFlags bindingFlags = BindingFlags.Default, Action<Exception> logAction = null) where T : class
        {
            return GetPropertiesOf(typeof(T), bindingFlags, logAction);
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
                encoding ??= Encoding.UTF8;

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
