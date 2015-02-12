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
        /// <param name="fileStream">The file stream that will be uploaded.</param>
        /// <param name="file">The file name that will be saved.</param>
        /// <param name="directory">The directory as to where the file will be saved.</param>
        /// <param name="allowedExtension">The extension that the file being uploaded should be. e.g. A text file would be 'txt'.</param>
        /// <param name="duplicateListingAction">What action to take when the path already exists..</param>
        /// <returns>The full path as to where the file has been uploaded.</returns>
        public static string WebUpload(Stream fileStream, string file, string directory, string allowedExtension, DuplicateListingActionOption duplicateListingAction = DuplicateListingActionOption.NoAction)
        {
            List<string> allowedExtensions = null;
            
            if (!string.IsNullOrWhiteSpace(allowedExtension)) {
                allowedExtensions = new List<string>();
                allowedExtensions.Add(allowedExtension);
            }

            return WebUpload(fileStream, file, directory, allowedExtensions, duplicateListingAction);
        }


        /// <summary>
        /// Upload a file to a specific directory.
        /// </summary>
        /// <param name="fileStream">The file stream that will be uploaded.</param>
        /// <param name="file">The file name that will be saved.</param>
        /// <param name="directory">The directory as to where the file will be saved.</param>
        /// <param name="allowedExtensions">A list of extensions that the file being uploaded should be. e.g. A text file would be 'txt'.</param>
        /// <param name="duplicateListingAction">What action to take when the path already exists..</param>
        /// <returns>The full path ass to where the file has been uploaded.</returns>
        public static string WebUpload(Stream fileStream, string file, string directory, List<string> allowedExtensions, DuplicateListingActionOption duplicateListingAction = DuplicateListingActionOption.NoAction)
        {            
            // File doesn't exist, so throw exception.
            if (file == null)
            {
                var placeholders = new List<string>();

                throw new ByteTurnNotFoundException(ErrorMessageOption.UPLOAD_FILE_NOT_EXISTS.ToErrorMessage(placeholders));
            }

            if (!Exists(directory))
            {
                var d = new DirectoryData(directory);

                // Create directory if it does not exist.
                Create(d.Name, d.Directory, ListingTypeOption.Directory, DuplicateListingActionOption.NoAction);
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
            var path = ListingExtensions.FormatDirectory(directory) + @"\" + file;

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

            else if (fileStream.Length > maxRequestLength)
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

                    throw new ByteTurnExistsException(ErrorMessageOption.UPLOAD_FILE_EXISTS.ToErrorMessage(placeholders));
                }

                try
                {
                    using (var f = File.Create(path))
                    {
                        fileStream.Seek(0, SeekOrigin.Begin);
                        fileStream.CopyTo(f);
                    }
                }
                catch (UnauthorizedAccessException)
                {                    
                    // Unable to save the file.
                    var placeholders = new List<string>();
                    placeholders.Add(directory);

                    throw new ByteTurnUnauthorisedAccessException(ErrorMessageOption.UPLOAD_FILE_UNAUTHORISED.ToErrorMessage(placeholders));
                }
                catch (Exception ex)
                {
                    // Unable to save the file.
                    var placeholders = new List<string>();
                    placeholders.Add(directory);

                    throw new ByteTurnException(ErrorMessageOption.UPLOAD_FILE_FAILURE.ToErrorMessage(placeholders), ex);
                }
            }

            return path;
        }
    }
}
