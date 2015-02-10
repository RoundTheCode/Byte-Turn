using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoundTheCode.ByteTurn.Services;
using RoundTheCode.ByteTurn.Data.Listing;
using System.IO;
using RoundTheCode.ByteTurn.Exception;
using System.Web;

namespace RoundTheCode.ByteTurn.Test
{
    [TestClass]
    public partial class ListingInformation
    {
        public string path = @"..\..\..\RoundTheCode.ByteTurn.Web\Test";
        public string parentPath = @"..\..\..\RoundTheCode.ByteTurn.Web";
        public string testPath = @"..\..\..\RoundTheCode.ByteTurn.Test\TestFiles";

        [TestInitializeAttribute]
        public void Init()
        {
            path = Path.GetFullPath(path);
            parentPath = Path.GetFullPath(parentPath);
            testPath = Path.GetFullPath(testPath);

            if (!ListingService.Exists(path))
            {
                ListingService.Create("Test", parentPath, Data.Listing.ListingTypeOption.Directory, Data.Listing.DuplicateListingActionOption.NoAction);
            }

            var deleteFiles = ListingService.GetListingByDirectory(path, false, true);

            foreach (var f in deleteFiles)
            {
                ListingService.Delete(f.FullFilePath);
            }

            var testFiles = ListingService.GetListingByDirectory(testPath, false, true);

            foreach (var f in testFiles)
            {
                ListingService.Copy(f.FullFilePath, path + @"\" + f.Name);
            }
        }

        [TestMethod, Priority(-1)]
        public void DirectoryInfo()
        {
            var d = new DirectoryData(path);

            Assert.AreEqual(d.Directory, parentPath);
            Assert.AreEqual(d.FullFilePath, path);
            Assert.AreEqual<ListingTypeOption>(d.ListingType, ListingTypeOption.Directory);
            Assert.AreEqual(d.Name, "Test");        
        }

        [TestMethod, Priority(-2)]
        public void FileInfo()
        {
            var f = new FileData(path + @"\testfile.txt");

            Assert.AreEqual(f.Directory, path);
            Assert.AreEqual(f.Extension, ".txt");
            Assert.AreEqual(f.FullFilePath, path + @"\testfile.txt");
            Assert.AreEqual<ListingTypeOption>(f.ListingType, ListingTypeOption.File);
            Assert.AreEqual(f.Name, "testfile.txt");
            Assert.AreEqual(f.Size.IsEqual(new FileSize(15)), true);
        }

        [TestMethod, Priority(-3)]
        public void Create()
        {
            try
            {
                ListingService.Create("testfile.txt", path, ListingTypeOption.File, DuplicateListingActionOption.NoAction);
                Assert.Fail();
            }
            catch (ByteTurnException byteturnex)
            {
                Assert.AreEqual(typeof(ByteTurnException), byteturnex.GetType());
            }

            var p = ListingService.Create("testfile-2.txt", path, ListingTypeOption.File, DuplicateListingActionOption.NoAction);

            Assert.AreEqual(p, path + @"\testfile-2.txt");
            Assert.AreEqual(ListingService.Exists(path + @"\testfile-2.txt"), true);

            p = ListingService.Create("testfile.txt", path, ListingTypeOption.File, DuplicateListingActionOption.AppendNumber);

            Assert.AreEqual(p, path + @"\(1) testfile.txt");
            Assert.AreEqual(ListingService.Exists(path + @"\(1) testfile.txt"), true);

            p = ListingService.Create("testfile.txt", path, ListingTypeOption.File, DuplicateListingActionOption.Overwrite);

            Assert.AreEqual(p, path + @"\testfile.txt");

            var f = new FileData(p);
            Assert.AreEqual(f.Size.IsEqual(new FileSize(0)), true);
        }

        [TestMethod, Priority(-4)]
        public void Delete()
        {
            // Delete non existance of file.
            try
            {
                ListingService.Delete(path + @"\delete.txt");
                Assert.Fail();
            }
            catch (ByteTurnException byteturnex)
            {
                Assert.AreEqual(typeof(ByteTurnException), byteturnex.GetType());
            }

            ListingService.Delete(path + @"\testfile-2.txt");
        }
    }
}
