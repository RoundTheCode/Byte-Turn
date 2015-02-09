using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoundTheCode.ByteTurn.Services;
using RoundTheCode.ByteTurn.Data.Listing;
using System.IO;

namespace RoundTheCode.ByteTurn.Test
{
    [TestClass]
    public partial class ListingInformation
    {
        public string path = @"..\..\..\RoundTheCode.ByteTurn.Web\Test";
        public string parentPath = @"..\..\..\RoundTheCode.ByteTurn.Web";


        public ListingInformation()
        {
            path = Path.GetFullPath(path);
            parentPath = Path.GetFullPath(parentPath);

            if (!ListingService.Exists(path))
            {
                ListingService.Create("Test", parentPath, Data.Listing.ListingTypeOption.Directory, Data.Listing.DuplicateListingActionOption.NoAction);
            }

            if (ListingService.Exists(path + "@\test.txt"))
            {
                ListingService.Delete(path + "@\test.txt");
            }

            ListingService.Create("test.txt", path, ListingTypeOption.File, DuplicateListingActionOption.NoAction);
        }

        [TestMethod]
        public void DirectoryInfo()
        {
            var d = new DirectoryData(path);

            Assert.AreEqual(d.Directory, parentPath);
            Assert.AreEqual(d.FullFilePath, path);
            Assert.AreEqual<ListingTypeOption>(d.ListingType, ListingTypeOption.Directory);
            Assert.AreEqual(d.Name, "Test");        
        }

        [TestMethod]
        public void FileInfo()
        {
            var f = new FileData(path + @"\test.txt");

            Assert.AreEqual(f.Directory, path);
            Assert.AreEqual(f.Extension, ".txt");
            Assert.AreEqual(f.FullFilePath, path + @"\test.txt");
            Assert.AreEqual<ListingTypeOption>(f.ListingType, ListingTypeOption.File);
            Assert.AreEqual(f.Name, "test.txt");
            Assert.AreEqual(f.Size.Bytes, 0);
            Assert.AreEqual(f.Size.Gigabytes, 0);
            Assert.AreEqual(f.Size.Kilobytes, 0);
            Assert.AreEqual(f.Size.Megabytes, 0);
            Assert.AreEqual(f.Size.Terabytes, 0);
        }
    }
}
