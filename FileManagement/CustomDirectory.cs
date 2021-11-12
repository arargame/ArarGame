using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Core.Model;
using FileManagement.Utils;

namespace FileManagement
{
    public interface ICustomDirectory: IBaseObject
    {
        string Text { get; set; }
        List<ICustomFile> Files { get; set; }
    }

    public class CustomDirectory : BaseObject,ICustomDirectory
    {
        #region Properties

        public string Text { get; set; }

        #endregion

        #region Collections

        public List<ICustomFile> Files { get; set; }

        #endregion

        #region CalculatedProperties

        public static string CurrentUserDesktopPath
        {
            get 
            {
                return GetSpecialFolderPath(Environment.SpecialFolder.Desktop);
            }
        }

        public static string CurrentProjectBinPath 
        {
            get 
            {
                return Environment.CurrentDirectory;
            }
        }

        #endregion

        #region Constructor

        public CustomDirectory()
        {
            Initialize();
        }

        public CustomDirectory(string path) : this()
        {
            SetText(path);

            Files = DirectoryGetFiles(path).Select(p => CustomFile.CreateFromPath(p)).ToList();
        }

        #endregion

        #region SetFunctions

        public ICustomDirectory AddFiles(List<ICustomFile> files)
        {
            Files.AddRange(files.Select(f=>f.SetCustomDirectory(this)));

            return this;
        }


        public ICustomDirectory SetText(string text)
        {
            Text = text;

            return this;
        }

        #endregion

        #region Functions

        public override IBaseObject Initialize()
        {
            Files = new List<ICustomFile>();

            return base.Initialize();
        }

        #endregion

        #region StaticFunctions

        public static string CombinePaths(params string[] paths)
        {
            var combinedPath = Path.Combine(paths);
            
            return combinedPath;
        }

        public static List<string> DirectoryGetFiles(string path)
        {
            var list = new List<string>();

            foreach (var file in Directory.GetFiles(path))
            {
                list.Add(file);
            }

            return list;
        }

        public static string GetSpecialFolderPath(Environment.SpecialFolder specialFolder)
        {
            return Environment.GetFolderPath(specialFolder);
        }

        public static bool HasInvalidPathChar(string path)
        {
            return Path.GetInvalidPathChars().Any(c => path.Contains(c));
        }

        #endregion
    }
}
