using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Model;
using FileManagement;
using FileManagement.Utils;
using Xunit;

namespace XUnitTest.Manager.FileManagementTest
{
    public class CustomFileTest
    {
        public string FileName { get; set; }

        public string Path { get; set; }

        public string WordToWrite { get; set; }
        public CustomFileTest()
        {
            FileName = "test.txt";

            Path = CustomDirectory.CombinePaths(CustomDirectory.CurrentProjectBinPath, FileName);

            WordToWrite = "Neque porro quisquam est qui dolorem ipsum quia dolor sit amet, consectetur, adipisci velit...";
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
        public ICustomFile CreateCustomFileFromPath()
        {
            var file = CustomFile.CreateFromPath(Path);

            Assert.NotNull(file);

            return file;
        }

        [Fact]
        public void CreateCustomFileWithEmptyConstructorUsingPath()
        {
            var file = new CustomFile().SetData(Path);

            Assert.NotNull(file);
        }

        [Fact]
        public void CreateCustomFileWithEmptyConstructorUsingData()
        {
            var data = Helper.StringToByteArray(WordToWrite);

            var file = new CustomFile().SetData(data);

            Assert.NotNull(file);
        }

        [Fact]
        public void SaveAs()
        {
            var file = CreateCustomFileFromPath();

            var data = Helper.StringToByteArray(WordToWrite + Guid.NewGuid());

            file.SetData(data);

            Assert.True(file.SaveAs());
        }

        [Fact]
        public void SaveAsWithPath()
        {
            var file = CreateCustomFileFromPath();

            var data = Helper.StringToByteArray(WordToWrite + Guid.NewGuid());

            file.SetData(data);

            var newPath = CustomDirectory.CombinePaths(CustomDirectory.CurrentProjectBinPath,"newfiletest.txt");

            file.SetPathToWrite(newPath);

            Assert.True(file.SaveAs());
        }

        [Fact]
        public void ZipUnZipTest()
        {
            var path = CustomDirectory.CombinePaths(CustomDirectory.CurrentProjectBinPath,"Files/Musics/sample.mp3");

            var file = CustomFile.CreateFromPath(path);

            var lengthAsMbBeforeZipped = file.LengthAsMb;

            file.Zip();

            var lengthAsMbAfterZipped = file.LengthAsMb;

            Assert.True(lengthAsMbAfterZipped < lengthAsMbBeforeZipped);

            file.UnZip();

            var lengthAsMbAfterUnZipped = file.LengthAsMb;

            Assert.True(lengthAsMbAfterUnZipped>lengthAsMbAfterZipped);

            Assert.True(lengthAsMbAfterUnZipped == lengthAsMbBeforeZipped);
        }
    }
}
