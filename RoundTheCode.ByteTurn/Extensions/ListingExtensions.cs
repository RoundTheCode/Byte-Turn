using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoundTheCode.ByteTurn.Extensions
{
    public static class ListingExtensions
    {
        public static string FormatDirectory(string dir)
        {
            if (!string.IsNullOrWhiteSpace(dir))
            {
                if (dir.Substring(dir.Length - 1, 1) == "/" || dir.Substring(dir.Length - 1, 1) == @"\")
                {
                    dir = dir.Substring(0, dir.Length - 1);
                }
            }

            return dir;
        }
    }
}
