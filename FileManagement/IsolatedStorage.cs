using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Core.Model;

namespace FileManagement
{
    public class IsolatedStorage : BaseObject,IDisposable
    {
        IsolatedStorageFile StorageFile { get; set; }

        public IsolatedStorageFileStream StorageFileStream { get; set; }

        public string IsolatedStoragePath {
            get 
            {
                return StorageFileStream.GetType()
                                        .GetField("m_FullPath", BindingFlags.Instance | BindingFlags.NonPublic)
                                        .GetValue(StorageFileStream)
                                        .ToString();
            }
        }


        public IsolatedStorage(string fileNameWithExtension)
        {
            try
            {
                StorageFile = IsolatedStorageFile.GetUserStoreForDomain();

                StorageFileStream = new IsolatedStorageFileStream(fileNameWithExtension, FileMode.OpenOrCreate, StorageFile);
            }
            catch (Exception ex)
            {
                if (LogAction != null)
                    LogAction(ex);
            }
        }


        ~IsolatedStorage()
        {
            Dispose();
        }


        public void Dispose()
        {
            if (StorageFile != null)
                StorageFile.Close();

            if (StorageFileStream != null)
                StorageFileStream.Close();
        }
    }
}
