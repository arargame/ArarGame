using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FileManagement.Utils
{
    public abstract class Helper : Core.Utils.Helper
    {
        public static string ByteArrayToString(byte[] data, Encoding encoding = null)
        {
            encoding = encoding ?? Encoding.UTF8;

            return encoding.GetString(data);
        }
    }
}
