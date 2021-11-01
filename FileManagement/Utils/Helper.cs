using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
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

        public static byte[] Compress(string text, Action<Exception> logAction = null)
        {
            var bytes = Encoding.UTF8.GetBytes(text);

            return Compress(bytes, logAction);
        }

        public static byte[] Compress(byte[] raw, Action<Exception> logAction = null)
        {
            try
            {
                using (MemoryStream memory = new MemoryStream())
                {
                    using (GZipStream gzip = new GZipStream(memory,
                        CompressionMode.Compress, true))
                    {
                        gzip.Write(raw, 0, raw.Length);
                    }

                    return memory.ToArray();
                }
            }
            catch (Exception ex)
            {
                logAction?.Invoke(ex);
            }

            return null;
        }

        public static byte[] Decompress(byte[] gzip, Action<Exception> logAction = null)
        {
            try
            {
                using (GZipStream stream = new GZipStream(new MemoryStream(gzip),CompressionMode.Decompress))
                {
                    const int size = 4096;

                    byte[] buffer = new byte[size];

                    using (MemoryStream memory = new MemoryStream())
                    {
                        int count = 0;

                        do
                        {
                            count = stream.Read(buffer, 0, size);

                            if (count > 0)
                            {
                                memory.Write(buffer, 0, count);
                            }
                        }
                        while (count > 0);

                        return memory.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                logAction?.Invoke(ex);
            }

            return null;
        }
    }
}
