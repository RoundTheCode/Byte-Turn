using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RoundTheCode.ByteTurn.Services;

namespace RoundTheCode.ByteTurn.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var a = ListingService.GetListingByDirectory(@"C:\inetpub\wwwroot");
        }
    }
}
