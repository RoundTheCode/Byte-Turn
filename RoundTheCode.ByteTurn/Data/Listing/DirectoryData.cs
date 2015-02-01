using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RoundTheCode.ByteTurn.Data.Listing
{
    public partial class DirectoryData : IListingData
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

        public DirectoryData(string fullFilePath)
        {
            FullFilePath = fullFilePath;

            ListingType = ListingTypeOption.Directory;

            var directoryInfo = new DirectoryInfo(fullFilePath);
            Name = directoryInfo.Name;
            Created = directoryInfo.CreationTimeUtc;
            Modified = directoryInfo.LastAccessTimeUtc;
            Directory = directoryInfo.Parent.FullName;
        }
    }
}
