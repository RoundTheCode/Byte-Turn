using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoundTheCode.ByteTurn.Exception
{
    public enum ErrorMessageOption
    {
        FILE_DIRECTORY_NOT_FOUND = 1,
        COPY_MOVE_FILE_DIRECTORY_EXISTS = 2,
        FILE_IN_USE = 3,
        UNAUTHORISED = 4,
        UNKNOWN = 5,
        PATH_TOO_LONG = 6,
        IO_ERROR = 7,
        NOT_SUPPORTED = 8,
        UPLOAD_ILLEGAL_FILE = 9,
        UPLOAD_FILE_FAILURE = 10,
        UPLOAD_FILE_NOT_EXISTS = 11,
        UPLOAD_FILE_TOO_LARGE = 12,
        UPLOAD_FILE_EXISTS = 13
    }
}
