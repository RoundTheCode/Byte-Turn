using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RoundTheCode.ByteTurn.Data.Listing
{
    public partial class FileData : IListingData
    {
        /// <summary>
        /// The full file path of the file or directory.
        /// </summary>
        public virtual string FullFilePath { get; protected set; }

        /// <summary>
        /// The name of the file or directory.
        /// </summary>
        public virtual string Name { get; protected set; }

        /// <summary>
        /// The directory where the file or directory resides.
        /// </summary>
        public virtual string Directory { get; protected set; }

        /// <summary>
        /// The date and time when the file or directory was created.
        /// </summary>
        public virtual DateTimeOffset Created { get; protected set; }

        /// <summary>
        /// The date and time when the file or directory was last modified.
        /// </summary>
        public virtual DateTimeOffset Modified { get; protected set; }

        /// <summary>
        /// The listing type. 1 = Directory, 2 = File.
        /// </summary>
        public virtual ListingTypeOption ListingType { get; protected set; }

        /// <summary>
        /// Gets the file size of the file.
        /// </summary>
        public virtual FileSize Size { get; protected set; }

        /// <summary>
        /// Gets the extension of the file.
        /// </summary>
        public virtual string Extension { get; protected set; }

        /// <summary>
        /// Creates a new instance of FileData.
        /// </summary>
        /// <param name="fullFilePath">The full path of the file name.</param>
        public FileData(string fullFilePath)
        {
            FullFilePath = fullFilePath;

            ListingType = ListingTypeOption.File;

            var fileInfo = new FileInfo(FullFilePath);
            Name = fileInfo.Name;
            Size = new FileSize(fileInfo.Length);
            Created = fileInfo.CreationTimeUtc;
            Modified = fileInfo.LastAccessTimeUtc;            
            Directory = fileInfo.DirectoryName;
            Extension = fileInfo.Extension;            
        }
    }
}
