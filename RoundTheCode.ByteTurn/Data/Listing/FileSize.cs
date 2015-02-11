using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoundTheCode.ByteTurn.Data.Listing
{
    public partial class FileSize
    {
        /// <summary>
        /// The file size in bytes.
        /// </summary>
        public long Bytes { get; protected set; }

        /// <summary>
        /// The file size in Kilobytes.
        /// </summary>
        public float Kilobytes { get; protected set; }

        /// <summary>
        /// The file size in Megabytes.
        /// </summary>
        public float Megabytes { get; protected set; }

        /// <summary>
        /// The file size in Gigabytes.
        /// </summary>
        public float Gigabytes { get; protected set; }

        /// <summary>
        /// The file size in Terabytes.
        /// </summary>
        public float Terabytes { get; protected set; }

        /// <summary>
        /// Creates a new FileSize instance.
        /// </summary>
        /// <param name="bytes">The file size in bytes.</param>
        public FileSize(long bytes) {
            Bytes = bytes;

            Kilobytes = (float)bytes / 1024;
            Megabytes = (float)Bytes / 1024 / 1024;
            Gigabytes = (float)Bytes / 1024 / 1024 / 1024;
            Terabytes = (float)Bytes / 1024 / 1024 / 1024 / 1024;
        }

        /// <summary>
        /// Compare an object to see if they are equal. Used for test plans.
        /// </summary>
        /// <param name="compareFileSize">Parse in an instance of FileSize to see if it equals this instance.</param>
        /// <returns>True if the File Size property matches. False if not.</returns>
        public bool IsEqual(FileSize compareFileSize)
        {
            return (this.Bytes == compareFileSize.Bytes && this.Kilobytes == compareFileSize.Kilobytes && this.Megabytes == compareFileSize.Megabytes && this.Gigabytes == compareFileSize.Gigabytes && this.Terabytes == compareFileSize.Terabytes);
        }

    }
}
