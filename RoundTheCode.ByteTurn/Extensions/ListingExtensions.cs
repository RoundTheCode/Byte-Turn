using RoundTheCode.ByteTurn.Data.Listing;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web.Configuration;
using System.Xml.Linq;

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

        public static string FormatExtension(string extension)
        {
            if (!string.IsNullOrWhiteSpace(extension))
            {
                if (!extension.StartsWith("."))
                {
                    extension = "." + extension;
                }
            }
            return extension;
        }

        public static long GetMaxRequestLength()
        {
            var limit = (long)30000000;

            Configuration configuration = null;

            try
            {
                configuration = WebConfigurationManager.OpenWebConfiguration("~");
            }
            catch (System.Exception)
            {
                configuration = null;
            }
            if (configuration != null)
            {
                var section = configuration.GetSection("system.webServer");

                if (section != null)
                {
                    long trylimit = 0;

                    var xml = section.SectionInformation.GetRawXml();
                    var doc = XDocument.Parse(xml);

                    if (doc.Root != null && doc.Root.Element("security") != null && doc.Root.Element("security").Element("requestFiltering") != null && doc.Root.Element("security").Element("requestFiltering").Element("requestLimits") != null && doc.Root.Element("security").Element("requestFiltering").Element("requestLimits").Attribute("maxAllowedContentLength") != null && long.TryParse(doc.Root.Element("security").Element("requestFiltering").Element("requestLimits").Attribute("maxAllowedContentLength").Value, out trylimit))
                    {
                        limit = trylimit;
                    }
                }
                else
                {
                    HttpRuntimeSection s2 = ConfigurationManager.GetSection("system.web/httpRuntime") as HttpRuntimeSection;
                    if (s2 != null)
                    {
                        limit = s2.MaxRequestLength;
                    }
                }
            }
            return limit;
        }

        public static string GetSizeTitle(long bytes)
        {
            var size = new FileSize(bytes);

            var sizeConversion = (long)1024;

            if (size.Bytes < sizeConversion)
            {
                return bytes + " bytes";
            }
            else if (size.Bytes < (sizeConversion * sizeConversion))
            {
                return size.Kilobytes.ToString("0.0") + " KB";
            }
            else if (size.Bytes < (sizeConversion * sizeConversion * sizeConversion))
            {
                return size.Megabytes.ToString("0.0") + " MB";
            }
            else if (size.Bytes < (sizeConversion * sizeConversion * sizeConversion * sizeConversion))
            {
                return size.Gigabytes.ToString("0.0") + " GB";
            }
            else
            {
                return size.Terabytes.ToString("0.0") + " TB";
            }
        }
    }
}
