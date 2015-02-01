using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoundTheCode.ByteTurn.Data.Listing
{
    /// <summary>
    /// How to deal with a duplicate file or listing.
    /// </summary>
    public enum DuplicateListingActionOption
    {
        NoAction = 0,
        Overwrite = 1,
        AppendNumber = 2
    }
}
