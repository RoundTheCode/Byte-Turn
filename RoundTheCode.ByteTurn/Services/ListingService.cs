using RoundTheCode.ByteTurn.Data.Listing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using RoundTheCode.ByteTurn.Exception;
using RoundTheCode.ByteTurn.Extensions;

namespace RoundTheCode.ByteTurn.Services
{
    using Exception = System.Exception;
    using System.Security.AccessControl;
using System.Web;

    public static partial class ListingService
    {
        /// <summary>
        /// What to do when a file or directory being moved or copied already exists in it's specified destination path.
        /// </summary>
        /// <param name="destPath">The full path of where the file or directory is going to be copied or moved.</param>
        /// <param name="duplicateListingAction">What action to take when the destination path already exists.</param>
        /// <returns></returns>
        static string DuplicateListingActions(string destPath, DuplicateListingActionOption duplicateListingAction)
        {
            if (duplicateListingAction == DuplicateListingActionOption.Overwrite && Exists(destPath))
            {
                Delete(destPath);
            }
            else if (duplicateListingAction == DuplicateListingActionOption.AppendNumber && Exists(destPath))
            {
                var listing = ListingService.GetListing(destPath);

                int tt = 1;

                while (Exists(destPath))
                {
                    if (listing != null)
                    {
                        destPath = listing.Directory + @"\(" + tt + ") " + listing.Name;
                    }
                    tt++;
                }
            }

            return destPath;
        }

        /// <summary>
        /// Delete a file or directory.
        /// </summary>
        /// <param name="path">The full path of the file or directory to be deleted.</param>
        public static void Delete(string path)
        {
            var placeHolders = new List<string>();
            placeHolders.Add("Unable to delete the file or directory '" + path + ". ");

            if (File.Exists(path))
            {
                try
                {
                    File.Delete(path);
                }
                catch (IOException)
                {
                    throw new ByteTurnException(ErrorMessageOption.FILE_IN_USE.ToErrorMessage(placeHolders));
                }
                catch (UnauthorizedAccessException)
                {
                    throw new ByteTurnException(ErrorMessageOption.UNAUTHORISED.ToErrorMessage(placeHolders));
                }
                catch (Exception ex)
                {
                    throw new ByteTurnException(ErrorMessageOption.UNKNOWN.ToErrorMessage(placeHolders), ex);
                }
            }
            else if (Directory.Exists(path))
            {
                try
                {
                    Directory.Delete(path);
                }
                catch (IOException)
                {
                    throw new ByteTurnException(ErrorMessageOption.FILE_IN_USE.ToErrorMessage(placeHolders));
                }
                catch (UnauthorizedAccessException)
                {
                    throw new ByteTurnException(ErrorMessageOption.UNAUTHORISED.ToErrorMessage(placeHolders));
                }
                catch (Exception ex)
                {
                    throw new ByteTurnException(ErrorMessageOption.UNKNOWN.ToErrorMessage(placeHolders), ex);
                }
            }
            else
            {


                throw new ByteTurnException(ErrorMessageOption.FILE_DIRECTORY_NOT_FOUND.ToErrorMessage(placeHolders));
            }
        }

        /// <summary>
        /// Does the file or directory exist?
        /// </summary>
        /// <param name="path">The full path of the file or directory.</param>
        /// <returns>True if the file or directory exists. False if neither the directory or file exists.</returns>
        public static bool Exists(string path)
        {
            try
            {
                return File.Exists(path) || Directory.Exists(path);
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Copy a file to a different path.
        /// </summary>
        /// <param name="currentPath">The current path of the file.</param>
        /// <param name="destPath">The destination path of the file.</param>
        /// <param name="duplicateListingAction">What to do if the destination path of the file exists.</param>
        /// <returns>The path name as to where the file was copied to.</returns>
        public static string Copy(string currentPath, string destPath, DuplicateListingActionOption duplicateListingAction = DuplicateListingActionOption.NoAction)
        {
            destPath = DuplicateListingActions(destPath, duplicateListingAction);

            var placeHolders = new List<string>();
            placeHolders.Add("Unable to copy the file '" + currentPath + "' to '" + destPath + ". ");

            if (!File.Exists(currentPath))
            {
                throw new ByteTurnException(ErrorMessageOption.FILE_DIRECTORY_NOT_FOUND.ToErrorMessage(placeHolders));
            }
            else if (Exists(destPath))
            {
                throw new ByteTurnException(ErrorMessageOption.COPY_MOVE_FILE_DIRECTORY_EXISTS.ToErrorMessage(placeHolders));
            }
            else if (File.Exists(currentPath) && !Exists(destPath))
            {
                try
                {
                    File.Copy(currentPath, destPath);
                }
                catch (UnauthorizedAccessException)
                {
                    throw new ByteTurnException(ErrorMessageOption.UNAUTHORISED.ToErrorMessage(placeHolders));
                }
                catch (PathTooLongException)
                {
                    throw new ByteTurnException(ErrorMessageOption.PATH_TOO_LONG.ToErrorMessage(placeHolders));
                }
                catch (IOException)
                {
                    throw new ByteTurnException(ErrorMessageOption.IO_ERROR.ToErrorMessage(placeHolders));
                }
                catch (NotSupportedException)
                {
                    throw new ByteTurnException(ErrorMessageOption.NOT_SUPPORTED.ToErrorMessage(placeHolders));
                }
                catch (Exception ex)
                {
                    throw new ByteTurnException(ErrorMessageOption.UNKNOWN.ToErrorMessage(placeHolders), ex);
                }
            }

            return destPath;
        }

        /// <summary>
        /// Create a file or directory.
        /// </summary>
        /// <param name="name">The name of the file or directory that is going to be created.</param>
        /// <param name="path">The full directory path of where the file or directory is going to be created.</param>
        /// <param name="listingType">Whether it's a file or directory that is going to be created.</param>
        /// <param name="duplicateListingAction">What to do if the path of the file or directory already exists.</param>
        /// <returns>The path name as to where the file was created to.</returns>
        public static string Create(string name, string path, ListingTypeOption listingType, DuplicateListingActionOption duplicateListingAction = DuplicateListingActionOption.NoAction)
        {
            string originalPath = path;

            var placeHolders = new List<string>();


            if (!path.EndsWith(@"\") && !path.EndsWith("/"))
                path += @"\";           

            path += name;

            path = DuplicateListingActions(path, duplicateListingAction);

            if (!Exists(path))
            {
                if (listingType == ListingTypeOption.Directory)
                {
                    placeHolders.Add("Unable to create the directory '" + path + "'. ");

                    try
                    {
                        Directory.CreateDirectory(path);
                    }
                    catch (UnauthorizedAccessException)
                    {
                        throw new ByteTurnException(ErrorMessageOption.UNAUTHORISED.ToErrorMessage(placeHolders));
                    }
                    catch (PathTooLongException)
                    {
                        throw new ByteTurnException(ErrorMessageOption.PATH_TOO_LONG.ToErrorMessage(placeHolders));
                    }
                    catch (NotSupportedException)
                    {
                        throw new ByteTurnException(ErrorMessageOption.NOT_SUPPORTED.ToErrorMessage(placeHolders));
                    }
                    catch (Exception ex)
                    {
                        throw new ByteTurnException(ErrorMessageOption.UNKNOWN.ToErrorMessage(placeHolders), ex);
                    }
                }
                else if (listingType == ListingTypeOption.File)
                {
                    placeHolders.Add("Unable to create the file '" + name + "' into '" + originalPath + ". ");
                    placeHolders.Add(", the file is in use, or the file is read only");

                    if (Directory.Exists(originalPath))
                    {
                        try
                        {
                            var f = File.Create(path);
                            f.Close();
                        }
                        catch (UnauthorizedAccessException)
                        {
                            throw new ByteTurnException(ErrorMessageOption.UNAUTHORISED.ToErrorMessage(placeHolders));
                        }
                        catch (PathTooLongException)
                        {
                            throw new ByteTurnException(ErrorMessageOption.PATH_TOO_LONG.ToErrorMessage(placeHolders));
                        }
                        catch (NotSupportedException)
                        {
                            throw new ByteTurnException(ErrorMessageOption.NOT_SUPPORTED.ToErrorMessage(placeHolders));
                        }
                        catch (Exception ex)
                        {
                            throw new ByteTurnException(ErrorMessageOption.UNKNOWN.ToErrorMessage(placeHolders), ex);
                        }
                    }
                    else
                    {
                        throw new ByteTurnException(ErrorMessageOption.FILE_DIRECTORY_NOT_FOUND.ToErrorMessage(placeHolders));
                    }
                }
            }
            else
            {
                if (listingType == ListingTypeOption.File)
                {
                    var placeholders = new List<string>();
                    placeholders.Add(path);

                    throw new ByteTurnException(ErrorMessageOption.CREATE_FILE_EXISTS.ToErrorMessage(placeholders));
                }
                else
                {
                    var placeholders = new List<string>();
                    placeholders.Add(path);

                    throw new ByteTurnException(ErrorMessageOption.CREATE_DIRECTORY_EXISTS.ToErrorMessage(placeholders));
                }                
            }

            return path;
        }

        /// <summary>
        /// Move a file or directory to a new location. 
        /// </summary>
        /// <param name="currentPath">The full path of the current file or directory.</param>
        /// <param name="destPath">The full path of the destination file or directory.</param>
        /// <param name="duplicateListingAction">What to do if the path of the file or directory already exists.</param>
        /// <returns>The path name as to where the file was moved to.</returns>
        public static string Move(string currentPath, string destPath, DuplicateListingActionOption duplicateListingAction = DuplicateListingActionOption.NoAction)
        {
            var placeHolders = new List<string>();
            placeHolders.Add("Unable to move the file or directory '" + currentPath + "' to '" + destPath + ". ");

            destPath = DuplicateListingActions(destPath, duplicateListingAction);

            if (!File.Exists(currentPath) && !Directory.Exists(currentPath))
            {
                throw new ByteTurnException(ErrorMessageOption.FILE_DIRECTORY_NOT_FOUND.ToErrorMessage(placeHolders));
            }
            else if (Exists(destPath))
            {
                throw new ByteTurnException(ErrorMessageOption.COPY_MOVE_FILE_DIRECTORY_EXISTS.ToErrorMessage(placeHolders));
            }
            else if (File.Exists(currentPath) && !Exists(destPath))
            {
                try
                {
                    File.Move(currentPath, destPath);
                }
                catch (UnauthorizedAccessException)
                {
                    throw new ByteTurnException(ErrorMessageOption.UNAUTHORISED.ToErrorMessage(placeHolders));
                }
                catch (PathTooLongException)
                {
                    throw new ByteTurnException(ErrorMessageOption.PATH_TOO_LONG.ToErrorMessage(placeHolders));
                }
                catch (IOException)
                {
                    throw new ByteTurnException(ErrorMessageOption.IO_ERROR.ToErrorMessage(placeHolders));
                }
                catch (NotSupportedException)
                {
                    throw new ByteTurnException(ErrorMessageOption.NOT_SUPPORTED.ToErrorMessage(placeHolders));
                }
                catch (Exception ex)
                {
                    throw new ByteTurnException(ErrorMessageOption.UNKNOWN.ToErrorMessage(placeHolders), ex);
                }
            }
            else if (Directory.Exists(currentPath) && !File.Exists(destPath) && !Directory.Exists(destPath))
            {
                try
                {
                    Directory.Move(currentPath, destPath);
                }
                catch (UnauthorizedAccessException)
                {
                    throw new ByteTurnException(ErrorMessageOption.UNAUTHORISED.ToErrorMessage(placeHolders));
                }
                catch (PathTooLongException)
                {
                    throw new ByteTurnException(ErrorMessageOption.PATH_TOO_LONG.ToErrorMessage(placeHolders));
                }
                catch (IOException)
                {
                    throw new ByteTurnException(ErrorMessageOption.IO_ERROR.ToErrorMessage(placeHolders));
                }
                catch (NotSupportedException)
                {
                    throw new ByteTurnException(ErrorMessageOption.NOT_SUPPORTED.ToErrorMessage(placeHolders));
                }
                catch (Exception ex)
                {
                    throw new ByteTurnException(ErrorMessageOption.UNKNOWN.ToErrorMessage(placeHolders), ex);
                }
            }

            return destPath;
        }



        /// <summary>
        /// Gets file or directory info depending if it exists.
        /// </summary>
        /// <param name="path">The full path of the file or directory.</param>
        /// <returns>If listing is found, it will return an instance of ListingData. If the listing is not found, it will return null.</returns>
        public static IListingData GetListing(string path)
        {
            if (File.Exists(path))
            {
                return new FileData(path);
            }
            else if (Directory.Exists(path))
            {
                return new DirectoryData(path);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets a list of the file and directories within the specified path.
        /// </summary>
        /// <param name="path">The full path of the directory.</param>
        /// <param name="showDirectories">Whether to show directories in the list.</param>
        /// <param name="showFiles">Whether to show files in the list.</param>
        /// <returns>The full listing of the files and directories within a given directory.</returns>
        public static List<IListingData> GetListingByDirectory(string path, bool showDirectories = true, bool showFiles = true)
        {
            var files = Directory.GetFiles(path);
            var directories = Directory.GetDirectories(path);

            var ListingData = new List<IListingData>();

            if (showDirectories)
            {
                foreach (var d in directories)
                {
                    ListingData.Add(GetListing(d));
                }
            }

            if (showFiles)
            {
                foreach (var f in files)
                {
                    ListingData.Add(GetListing(f));
                }
            }

            return ListingData.OrderBy(x => (int)x.ListingType).ThenBy(x => x.Name).ToList();
        }
    }
}
