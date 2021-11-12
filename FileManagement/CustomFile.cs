using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Model;
using FileManagement.Utils;

namespace FileManagement
{
    public interface ICustomFile : IBaseObject
    {
        ICustomFile ClearData();
        byte[] Data { get; set; }
        string DataAsString { get; }
        ICustomDirectory Directory { get; set; }
        IBaseObject Entity { get; set; }

        string Extension { get; }        

        FileInfo Info { get; set; }


        double LengthAsGb { get; }

        double LengthAsMb { get; }

        double LengthAsKb { get; }

        double LengthAsByte { get; }

        string NameWithoutExtension { get; }

        bool SaveAs();

        bool SaveAs(string path);

        public ICustomFile Zip();

        public ICustomFile UnZip();

        ICustomFile SetCustomDirectory(ICustomDirectory customDirectory);

        ICustomFile SetData(byte[] data);

        ICustomFile SetFileInfo(string pathEndsWithFile);

        ICustomFile SetPathToRead(string path);

        ICustomFile SetPathToWrite(string path);
    }

    public class CustomFile : BaseObject,ICustomFile
    {
        #region Properties
        public byte[] Data { get; set; }

        public ICustomDirectory Directory { get; set; }
        public IBaseObject Entity { get; set; }

        public FileInfo Info { get; set; }

        public string PathToRead { get; private set; }

        public string PathToWrite { get; private set; }

        #endregion


        #region CalculatedProperties
        public string Base64String 
        {
            get 
            {
                return Convert.ToBase64String(Data);
            }
        }

        public virtual string Extension 
        {
            get 
            {
                return GetExtensionFromFileName(Name);
            }
        }

        public string DataAsString
        {
            get 
            {
                return Helper.ByteArrayToString(Data);
            }
        }

        public double LengthAsGb 
        {
            get 
            {
                return Convert.ToDouble(string.Format("{0:0.0000}", (double)LengthAsMb / 1024));
            }
        }

        public double LengthAsMb 
        {
            get 
            {
                return Convert.ToDouble(string.Format("{0:0.0000}", (double)LengthAsKb / 1024));
            }
        }

        public double LengthAsKb 
        {
            get 
            {
                return Convert.ToDouble(string.Format("{0:0.0000}", (double)LengthAsByte / 1024));
            }
        }

        public double LengthAsByte 
        {
            get 
            {
                return Convert.ToDouble(string.Format("{0:0.0000}", (double)(Data != null ? Data.Length : Info.Length)));
            }
        }

        public virtual string NameWithoutExtension
        {
            get 
            {
                return GetFileNameWithoutExtension(Name);
            }
        }

        #endregion

        #region Constructor

        public CustomFile()
        {

        }

        #endregion

        #region SetFunctions

        public ICustomFile ClearData()
        {
            Data = null;

            return this;
        }

        public ICustomFile SetCustomDirectory(ICustomDirectory customDirectory)
        {
            Directory = customDirectory;

            return this;
        }

        public ICustomFile SetData(byte[] data)
        {
            if (data != null)
                Data = data;

            return this;
        }

        public ICustomFile SetData(string path = null,bool createIfNotExist = true,Action<Exception> logAction = null)
        {
            try
            {
                SetPathToRead(path);

                SetPathToWrite(path);

                if (createIfNotExist)
                    CreateNewFileIfNotExists(path, logAction);

                SetFileInfo(path);

                Data = ReadAllBytes(path, logAction);
            }
            catch (Exception ex)
            {
                LogAction?.Invoke(ex);
            }

            return this;
        }

        public ICustomFile SetFileInfo(string pathEndsWithFile)
        {
            Info = new FileInfo(pathEndsWithFile);

            if(string.IsNullOrEmpty(Name))
                SetName(Info.Name);

            return this;
        }

        public ICustomFile SetPathToRead(string path)
        {
            if (!string.IsNullOrEmpty(path))
                PathToRead = path;

            return this;
        }

        public ICustomFile SetPathToWrite(string path)
        {
            if (!string.IsNullOrEmpty(path))
                PathToWrite = path;

            return this;
        }

        public new ICustomFile SetName(string name)
        {
            return (ICustomFile)base.SetName(name);
        }

        #endregion

        #region Functions

        public bool SaveAs()
        {
            return WriteAllBytes(PathToWrite,Data);
        }

        public bool SaveAs(string path)
        {
            return WriteAllBytes(path,Data);
        }

        public bool WriteAllBytes(string path = null, byte[] data = null)
        {
            SetPathToWrite(path);

            SetData(data);

            return WriteAllBytes(PathToWrite, data, LogAction);
        }

        public ICustomFile Zip()
        {
            return SetData(Helper.Compress(Data, LogAction));
        }

        public ICustomFile UnZip()
        {
            return SetData(Helper.Decompress(Data, LogAction));
        }

        #endregion

        #region StaticFunctions

        public static bool CreateNewFileIfNotExists(string path, Action<Exception> logAction = null)
        {
            try
            {
                
                if (!FileExists(path))
                {
                    using var fs = File.Create(path);
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                logAction?.Invoke(ex);

                return false;
            }
            catch (Exception ex)
            {
                logAction?.Invoke(ex);

                return false;
            }

            return true;
        }

        public static ICustomFile CreateFromPath(string path)
        {
            return new CustomFile().SetData(path);
        }

        public static string GetExtensionFromFileName(string fileName, char seperator = '.')
        {
            return Helper.SplitThenTakeByIndex(fileName, -1, seperator);
        }

        public static string GetExtension(string path)
        {
            return Path.GetExtension(path);
        }

        public static string GetFileNameWithoutExtension(string fileName, char seperator = '.')
        {
            return Helper.SplitThenTakeByIndex(fileName, 0, seperator);
        }

        public static string GetFileNameWithoutExtension(string path)
        {
            return Path.GetFileNameWithoutExtension(path);
        }
        
        public static ICustomFile GetSpecialFolderPathFile(Environment.SpecialFolder specialFolder,string fileNameWithExtension)
        {
            var path = CustomDirectory.CombinePaths(CustomDirectory.GetSpecialFolderPath(specialFolder), fileNameWithExtension);

            return CreateFromPath(path);
        }

        public static bool FileExists(string path)
        {
            return File.Exists(path);
        }

        public static byte[] ReadAllBytes(string path, Action<Exception> logAction = null)
        {
            byte[] array = null;

            try
            {
                array = File.ReadAllBytes(path);
            }
            catch (Exception ex)
            {
                logAction?.Invoke(ex);
            }

            return array;
        }


        public static bool WriteAllBytes(string path, byte[] data, Action<Exception> logAction = null)
        {
            try
            {
                File.WriteAllBytes(path, data);
            }
            catch (Exception ex)
            {
                logAction?.Invoke(ex);

                return false;
            }

            return true;
        }

        #endregion
    }
}
