using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Model;
using FileManagement.Utils;

namespace FileManagement
{
    public interface ICustomFile : IBaseObject
    {
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

        ICustomFile SetCustomDirectory(ICustomDirectory customDirectory);

        ICustomFile SetData(byte[] data);

        ICustomFile SetFileInfo(string pathEndsWithFile);
    }

    public class CustomFile : BaseObject,ICustomFile
    {
        #region Properties
        public byte[] Data { get; set; }

        public ICustomDirectory Directory { get; set; }
        public IBaseObject Entity { get; set; }

        public FileInfo Info { get; set; }

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
                return Convert.ToDouble(string.Format("{0:0.0000}", (double)Info.Length));
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

        public ICustomFile SetCustomDirectory(ICustomDirectory customDirectory)
        {
            Directory = customDirectory;

            return this;
        }

        public ICustomFile SetData(byte[] data)
        {
            Data = data;

            return this;
        }

        public ICustomFile SetData(string path,bool createIfNotExist = true,Action<Exception> logAction = null)
        {
            try
            {
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

        public new ICustomFile SetName(string name)
        {
            return (ICustomFile)base.SetName(name);
        }

        #endregion

        #region Functions



        #endregion

        #region StaticFunctions

        public static bool CreateNewFileIfNotExists(string path, Action<Exception> logAction = null)
        {
            try
            {
                
                if (!FileExists(path))
                {
                    using (FileStream fs = File.Create(path))
                    {

                    }
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
