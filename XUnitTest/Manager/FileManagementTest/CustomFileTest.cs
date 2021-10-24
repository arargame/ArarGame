using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Model;
using FileManagement;
using Xunit;

namespace XUnitTest.Manager.FileManagementTest
{
    public class CustomFileTest
    {
        public string FileName { get; set; }

        public string Path { get; set; }
        public CustomFileTest()
        {
            FileName = "test.txt";

            Path = CustomDirectory.CombinePaths(CustomDirectory.CurrentProjectBinPath, FileName);
        }

        [Fact]
        public void CreateNewFileIfNotExists()
        {
            var isCreated = CustomFile.CreateNewFileIfNotExists(Path, (ex) => {
                Console.WriteLine(ex.Message);
            });

            Assert.True(isCreated);
        }

        [Fact]
        public void OpenFileWithEmptyConstructor()
        {
            var file = CustomFile.CreateFromPath(Path);

            var directory2 = new CustomDirectory();

            directory2.Add(collection: new List<ICustomFile>() { file},expression: directory2.Files);

            var directory = new CustomDirectory().Add<CustomDirectory,CustomFile,ICustomDirectory,ICustomFile>(new List<ICustomFile>() { file },cd=>cd.Files,cf=>cf.Directory);

            //new CustomDirectory().Add<CustomDirectory, CustomFile>(new List<ICustomFile>() { file }, c => c.Files, c => c.Directory);

            Assert.NotNull(file);
        }
    }
}
