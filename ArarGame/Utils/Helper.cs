using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Core.Utils
{
    public abstract class Helper
    {
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
