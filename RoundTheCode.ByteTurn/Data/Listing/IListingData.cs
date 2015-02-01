using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoundTheCode.ByteTurn.Data.Listing
{
    public partial interface IListingData
    {
        /// <summary>
        /// The full file path of the file or directory.
        /// </summary>
        string FullFilePath { get; }

        /// <summary>
        /// The name of the file or directory.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The directory where the file or directory resides.
        /// </summary>
        string Directory { get; }

        /// <summary>
        /// The date and time when the file or directory was created.
        /// </summary>
        DateTimeOffset Created { get; }

        /// <summary>
        /// The date and time when the file or directory was last modified.
        /// </summary>
        DateTimeOffset Modified { get; }

        /// <summary>
        /// The listing type. 1 = Directory, 2 = File.
        /// </summary>
        ListingTypeOption ListingType { get; }
    }
}
