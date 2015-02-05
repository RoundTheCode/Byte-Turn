using RoundTheCode.ByteTurn.Exception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace RoundTheCode.ByteTurn.Extensions
{
    public static class ErrorMessageExtensions
    {
        public static string ToErrorMessage(this ErrorMessageOption error, List<string> placeholders)
        {
            switch (error)
            {
                case ErrorMessageOption.FILE_DIRECTORY_NOT_FOUND:
                    return ReturnErrorMessage("{p[0]}The file or directory cannot be found.",placeholders);
                case ErrorMessageOption.COPY_MOVE_FILE_DIRECTORY_EXISTS:
                    return ReturnErrorMessage("{p[0]}The file or directory being moved or copied already exists.", placeholders);
                case ErrorMessageOption.FILE_IN_USE:
                    return ReturnErrorMessage("{p[0]}Unauthorised access to perform that action. The specified file is in use.", placeholders);
                case ErrorMessageOption.UNAUTHORISED:
                    return ReturnErrorMessage("{p[0]}Unauthorised access to perform that action. This may be due to insufficient permissions, the file is in use, or the file is read only.", placeholders);
                case ErrorMessageOption.UNKNOWN:
                    return ReturnErrorMessage("{p[0]}An unknown error occured.", placeholders);
                case ErrorMessageOption.PATH_TOO_LONG:
                    return ReturnErrorMessage("{p[0]}The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters.", placeholders);
                case ErrorMessageOption.IO_ERROR:
                    return ReturnErrorMessage("{p[0]}An I/O error has occured.", placeholders);
                case ErrorMessageOption.NOT_SUPPORTED:
                    return ReturnErrorMessage("{p[0]}The path name is in an invalid format.", placeholders);
                case ErrorMessageOption.UPLOAD_ILLEGAL_FILE:
                    return ReturnErrorMessage("Unable to upload the file. The file being uploaded needs to have an extension of '{p[0]}'.", placeholders);
                case ErrorMessageOption.UPLOAD_FILE_FAILURE:
                    return ReturnErrorMessage("Unable to upload the file. This is likely due to a permission issue.", placeholders);
            }
            return "";
        }

        private static string ReturnErrorMessage(string message, List<string> placeholders)
        {
            if (placeholders != null && message.Contains("{p"))
            {
                for (var tt = 0; tt < placeholders.Count; tt++)
                {
                    message = message.Replace("{p[" + tt + "]}", placeholders[0]);
                }
            }

            if (!string.IsNullOrWhiteSpace(message) && message.Contains("{p"))
            {
                message = Regex.Replace(message, @"\{p\[([0-9]+)\]\}","");
            }

            return message;
        }
    }
}
