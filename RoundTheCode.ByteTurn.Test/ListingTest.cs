using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoundTheCode.ByteTurn.Services;
using RoundTheCode.ByteTurn.Data.Listing;
using System.IO;
using RoundTheCode.ByteTurn.Exception;
using System.Web;
using System.Collections.Generic;

namespace RoundTheCode.ByteTurn.Test
{
    [TestClass]
    public partial class ListingTest
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

            var deleteFiles = ListingService.GetListingByDirectory(path, true, true);

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
            catch (ByteTurnExistsException byteturnex)
            {
                Assert.AreEqual(typeof(ByteTurnExistsException), byteturnex.GetType());
            }

            // Illegal characters.
            try
            {
                ListingService.Create("testfile|.txt", path, ListingTypeOption.File, DuplicateListingActionOption.NoAction);
                Assert.Fail();
            }
            catch (ByteTurnNotSupportedException byteturnex)
            {
                Assert.AreEqual(typeof(ByteTurnNotSupportedException), byteturnex.GetType());
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
        public void Copy()
        {
            var p = ListingService.Create("testfile-4.txt", path, ListingTypeOption.File, DuplicateListingActionOption.NoAction);

            // No action - file does not exist.
            var f = ListingService.Copy(path + @"\testfile-4.txt", path + @"\testfile-42.txt", DuplicateListingActionOption.NoAction);
            Assert.AreEqual(f, path + @"\testfile-42.txt");
            Assert.AreEqual(ListingService.Exists(path + @"\testfile-42.txt"), true);

            // Illegal characters.
            try
            {
                f = ListingService.Copy(path + @"\testfile-4|.txt", path + @"\testfile-42|.txt", DuplicateListingActionOption.NoAction);
                Assert.Fail();
            }
            catch (ByteTurnNotSupportedException byteturnex)
            {
                Assert.AreEqual(typeof(ByteTurnNotSupportedException), byteturnex.GetType());
            }

            // No action - file exists.
            try
            {
                f = ListingService.Copy(path + @"\testfile-4.txt", path + @"\testfile-42.txt", DuplicateListingActionOption.NoAction);
                Assert.Fail();
            }
            catch (ByteTurnExistsException byteturnex)
            {
                Assert.AreEqual(typeof(ByteTurnExistsException), byteturnex.GetType());
            }

            // Overwrite
            f = ListingService.Copy(path + @"\testfile-4.txt", path + @"\testfile-42.txt", DuplicateListingActionOption.Overwrite);
            Assert.AreEqual(f, path + @"\testfile-42.txt");
            Assert.AreEqual(ListingService.Exists(path + @"\testfile-42.txt"), true);

            // Append number
            f = ListingService.Copy(path + @"\testfile-4.txt", path + @"\testfile-42.txt", DuplicateListingActionOption.AppendNumber);
            Assert.AreEqual(f, path + @"\(1) testfile-42.txt");
            Assert.AreEqual(ListingService.Exists(path + @"\(1) testfile-42.txt"), true);
        }

        [TestMethod, Priority(-5)]
        public void Delete()
        {
            // Delete non existance of file.
            try
            {
                ListingService.Delete(path + @"\delete.txt");
                Assert.Fail();
            }
            catch (ByteTurnNotFoundException byteturnex)
            {
                Assert.AreEqual(typeof(ByteTurnNotFoundException), byteturnex.GetType());
            }

            // Illegal characters.
            try
            {
                ListingService.Delete(path + @"\dele|te.txt");
                Assert.Fail();
            }
            catch (ByteTurnNotSupportedException byteturnex)
            {
                Assert.AreEqual(typeof(ByteTurnNotSupportedException), byteturnex.GetType());
            }

            var p = ListingService.Create("testfile-3.txt", path, ListingTypeOption.File, DuplicateListingActionOption.NoAction);

            ListingService.Delete(path + @"\testfile-3.txt");
        }

        [TestMethod, Priority(-6)]
        public void Move()
        {
            // Create 3 text files.
            var p = ListingService.Create("testfile-5.txt", path, ListingTypeOption.File, DuplicateListingActionOption.NoAction);
            ListingService.Create("testfile-51.txt", path, ListingTypeOption.File, DuplicateListingActionOption.NoAction);
            ListingService.Create("testfile-53.txt", path, ListingTypeOption.File, DuplicateListingActionOption.NoAction);

            // No action - file does not exist.
            var f = ListingService.Move(path + @"\testfile-5.txt", path + @"\testfile-52.txt", DuplicateListingActionOption.NoAction);
            Assert.AreEqual(f, path + @"\testfile-52.txt");            
            Assert.AreEqual(ListingService.Exists(path + @"\testfile-52.txt"), true);
            Assert.AreEqual(ListingService.Exists(path + @"\testfile-5.txt"), false);

            // No action - file exists.
            try
            {
                f = ListingService.Move(path + @"\testfile-51.txt", path + @"\testfile-52.txt", DuplicateListingActionOption.NoAction);
                Assert.Fail();
            }
            catch (ByteTurnExistsException byteturnex)
            {
                Assert.AreEqual(typeof(ByteTurnExistsException), byteturnex.GetType());
            }

            // Illegal characters.
            try
            {
                f = ListingService.Move(path + @"\testf|ile-51.txt", path + @"\testfil|e-52.txt", DuplicateListingActionOption.NoAction);
                Assert.Fail();
            }
            catch (ByteTurnNotSupportedException byteturnex)
            {
                Assert.AreEqual(typeof(ByteTurnNotSupportedException), byteturnex.GetType());
            }

            // Overwrite
            f = ListingService.Move(path + @"\testfile-51.txt", path + @"\testfile-52.txt", DuplicateListingActionOption.Overwrite);
            Assert.AreEqual(f, path + @"\testfile-52.txt");
            Assert.AreEqual(ListingService.Exists(path + @"\testfile-52.txt"), true);
            Assert.AreEqual(ListingService.Exists(path + @"\testfile-51.txt"), false);

            // Append number
            f = ListingService.Move(path + @"\testfile-53.txt", path + @"\testfile-52.txt", DuplicateListingActionOption.AppendNumber);
            Assert.AreEqual(f, path + @"\(1) testfile-52.txt");
            Assert.AreEqual(ListingService.Exists(path + @"\(1) testfile-52.txt"), true);
            Assert.AreEqual(ListingService.Exists(path + @"\testfile-53.txt"), false);

        }

        [TestMethod, Priority(-7)]
        public void WebUpload()
        {
            var p = "";

            // Upload a PNG.
            using (var sf = new StreamReader(path + @"\BarcelonaCat.png"))
            {
                p = ListingService.Upload(sf.BaseStream, "BarcelonaCat-2.png", path, "png", DuplicateListingActionOption.NoAction);
                sf.Close();
            }

            Assert.AreEqual(p, path + @"\BarcelonaCat-2.png");
            Assert.AreEqual(ListingService.Exists(path + @"\BarcelonaCat-2.png"), true);


            // Can't overwrite
            try
            {
                using (var sf = new StreamReader(path + @"\BarcelonaCat.png"))
                {
                    p = ListingService.Upload(sf.BaseStream, "BarcelonaCat-2.png", path, "png", DuplicateListingActionOption.NoAction);
                    sf.Close();
                }
                Assert.Fail();
            }
            catch (ByteTurnExistsException ex)
            {
                Assert.AreEqual(typeof(ByteTurnExistsException), ex.GetType());
            }

            // Illegal characters.
            try
            {
                using (var sf = new StreamReader(path + @"\BarcelonaCat.png"))
                {
                    p = ListingService.Upload(sf.BaseStream, "Barce|lonaCat-2.png", path + "|", "png", DuplicateListingActionOption.NoAction);
                    sf.Close();
                }
                Assert.Fail();
            }
            catch (ByteTurnNotSupportedException ex)
            {
                Assert.AreEqual(typeof(ByteTurnNotSupportedException), ex.GetType());
            }


            // Try and upload a file as a gif, when only png's are accepted.
            try
            {
                using (var sf = new StreamReader(path + @"\BarcelonaCat.png"))
                {
                    p = ListingService.Upload(sf.BaseStream, "BarcelonaCat-2.gif", path, "png", DuplicateListingActionOption.NoAction);
                    sf.Close();
                }
                Assert.Fail();
            }
            catch (ByteTurnUploadFileException ex)
            {
                Assert.AreEqual(typeof(ByteTurnUploadFileException), ex.GetType());
            }

            // Try and upload a file as a png without a dot.
            try
            {
                using (var sf = new StreamReader(path + @"\BarcelonaCat.png"))
                {
                    p = ListingService.Upload(sf.BaseStream, "BarcelonaCatpng", path, "png", DuplicateListingActionOption.NoAction);
                    sf.Close();
                }
                Assert.Fail();
            }
            catch (ByteTurnUploadFileException ex)
            {
                Assert.AreEqual(typeof(ByteTurnUploadFileException), ex.GetType());
            }

            var allowedExtensions = new List<string>();
            allowedExtensions.Add("png");
            allowedExtensions.Add("gif");
            // Upload a PNG.
            using (var sf = new StreamReader(path + @"\BarcelonaCat.png"))
            {
                p = ListingService.Upload(sf.BaseStream, "BarcelonaCat-3.png", path, allowedExtensions, DuplicateListingActionOption.NoAction);
                sf.Close();
            }

            Assert.AreEqual(p, path + @"\BarcelonaCat-3.png");
            Assert.AreEqual(ListingService.Exists(path + @"\BarcelonaCat-3.png"), true);

            // Try and upload a file as a gif, when only png's are accepted.
            try
            {
                using (var sf = new StreamReader(path + @"\BarcelonaCat.png"))
                {
                    p = ListingService.Upload(sf.BaseStream, "BarcelonaCat-2.jpg", path, allowedExtensions, DuplicateListingActionOption.NoAction);
                    sf.Close();
                }
                Assert.Fail();
            }
            catch (ByteTurnUploadFileException ex)
            {
                Assert.AreEqual(typeof(ByteTurnUploadFileException), ex.GetType());
            }

            // Try and upload a file as a png without a dot.
            try
            {
                using (var sf = new StreamReader(path + @"\BarcelonaCat.png"))
                {
                    p = ListingService.Upload(sf.BaseStream, "BarcelonaCatpng", path, allowedExtensions, DuplicateListingActionOption.NoAction);
                    sf.Close();
                }
                Assert.Fail();
            }
            catch (ByteTurnUploadFileException ex)
            {
                Assert.AreEqual(typeof(ByteTurnUploadFileException), ex.GetType());
            }

            // Overwrite file.
            using (var sf = new StreamReader(path + @"\BarcelonaCat.png"))
            {
                p = ListingService.Upload(sf.BaseStream, "BarcelonaCat-2.png", path, "png", DuplicateListingActionOption.Overwrite);
                sf.Close();
            }

            Assert.AreEqual(p, path + @"\BarcelonaCat-2.png");
            Assert.AreEqual(ListingService.Exists(path + @"\BarcelonaCat-2.png"), true);


            // Append number.
            using (var sf = new StreamReader(path + @"\BarcelonaCat.png"))
            {
                p = ListingService.Upload(sf.BaseStream, "BarcelonaCat-2.png", path, "png", DuplicateListingActionOption.AppendNumber);
                sf.Close();
            }

            Assert.AreEqual(p, path + @"\(1) BarcelonaCat-2.png");
            Assert.AreEqual(ListingService.Exists(path + @"\(1) BarcelonaCat-2.png"), true);
        }

    }
}
