using RoundTheCode.ByteTurn.Data.Listing;
using RoundTheCode.ByteTurn.Exception;
using RoundTheCode.ByteTurn.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace RoundTheCode.ByteTurn.Services
{
    using System.IO;
    using Exception = System.Exception;

    public static partial class ListingService
    {
        /// <summary>
        /// Upload a file to a specific directory.
        /// </summary>
        /// <param name="context">The HTTP context. If running from a web application, this would be HttpContext.</param>
        /// <param name="file">The class that stores the file details.</param>
        /// <param name="directory">The class that stores the file details.</param>
        /// <param name="allowedExtension">The extension that the file being uploaded should be. e.g. A text file would be 'txt'.</param>
        /// <returns>The full path as to where the file has been uploaded.</returns>
        public static string WebUpload(HttpContextBase context, HttpPostedFileBase file, string directory, string allowedExtension, DuplicateListingActionOption duplicateListingAction = DuplicateListingActionOption.NoAction)
        {
            List<string> allowedExtensions = null;
            
            if (!string.IsNullOrWhiteSpace(allowedExtension)) {
                allowedExtensions = new List<string>();
                allowedExtensions.Add(allowedExtension);
            }
            
            return WebUpload(context, file, directory, allowedExtensions, duplicateListingAction);
        }


        /// <summary>
        /// Upload a file to a specific directory.
        /// </summary>
        /// <param name="context">The HTTP context. If running from a web application, this would be HttpContext.</param>
        /// <param name="file">The class that stores the file details.</param>
        /// <param name="directory">The class that stores the file details.</param>
        /// <param name="allowedExtensions">A list of extensions that the file being uploaded should be. e.g. A text file would be 'txt'.</param>
        /// <returns>The full path as to where the file has been uploaded.</returns>
        public static string WebUpload(HttpContextBase context, HttpPostedFileBase file, string directory, List<string> allowedExtensions, DuplicateListingActionOption duplicateListingAction = DuplicateListingActionOption.NoAction)
        {
            // File doesn't exist, so throw exception.
            if (file == null)
            {
                var placeholders = new List<string>();

                throw new ByteTurnUploadFileException(ErrorMessageOption.UPLOAD_FILE_NOT_EXISTS.ToErrorMessage(placeholders));
            }

            if (!Exists(directory))
            {
                // Create directory if it does not exist.
                Create("", directory, ListingTypeOption.Directory, DuplicateListingActionOption.NoAction);
            }

            // Ensure all allowed extensions have a dot next to them.
            if (allowedExtensions != null)
            {
                for (var tt = 1; tt <= allowedExtensions.Count; tt++)
                {
                    allowedExtensions[tt - 1] = ListingExtensions.FormatExtension(allowedExtensions[tt - 1]);
                }
            }

            var maxRequestLength = ListingExtensions.GetMaxRequestLength();

            // Convert file & directory to full path.
            var path = ListingExtensions.FormatDirectory(directory) + "/" + file.FileName;

            if (allowedExtensions == null || allowedExtensions.FirstOrDefault(x => x.ToLower() == path.Substring(path.Length - x.Length, x.Length).ToLower()) == null)
            {
                // Extension doesn't match what should be uploaded, so error.
                string extensionText = "";

                if (allowedExtensions != null)
                {
                    for (var tt = 1; tt <= allowedExtensions.Count; tt++)
                    {
                        if (tt > 1 && tt != allowedExtensions.Count)
                        {
                            extensionText += ", ";
                        }
                        if (tt > 1 && tt == allowedExtensions.Count)
                        {
                            extensionText += " or ";
                        }
                        extensionText += "'" + allowedExtensions[tt - 1] + "'";
                    }
                }

                var placeholders = new List<string>();
                placeholders.Add(extensionText);

                throw new ByteTurnUploadFileException(ErrorMessageOption.UPLOAD_ILLEGAL_FILE.ToErrorMessage(placeholders));
            }

            else if (file.ContentLength > maxRequestLength)
            {
                // File is too large to upload, so error.
                var placeholders = new List<string>();
 
                 placeholders.Add(ListingExtensions.GetSizeTitle(maxRequestLength));

                throw new ByteTurnUploadFileException(ErrorMessageOption.UPLOAD_FILE_TOO_LARGE.ToErrorMessage(placeholders));
            }

            else
            {
                // Do duplicate action and convert the path.
                path = DuplicateListingActions(path, duplicateListingAction);

                if (Exists(path))
                {
                    // If file exists, throw error.
                    var placeholders = new List<string>();
                    placeholders.Add(path);

                    throw new ByteTurnUploadFileException(ErrorMessageOption.UPLOAD_FILE_EXISTS.ToErrorMessage(placeholders));
                }

                try
                {
                    // Save the file.
                    file.SaveAs(path);
                }
                catch (UnauthorizedAccessException uaex)
                {                    
                    // Unable to save the file.
                    var placeholders = new List<string>();
                    placeholders.Add(directory);

                    throw new ByteTurnUploadFileException(ErrorMessageOption.UPLOAD_FILE_UNAUTHORISED.ToErrorMessage(placeholders), uaex);
                }
                catch (Exception ex)
                {
                    // Unable to save the file.
                    var placeholders = new List<string>();
                    placeholders.Add(directory);

                    throw new ByteTurnUploadFileException(ErrorMessageOption.UPLOAD_FILE_FAILURE.ToErrorMessage(placeholders), ex);
                }
            }

            return path;
        }
    }
}
