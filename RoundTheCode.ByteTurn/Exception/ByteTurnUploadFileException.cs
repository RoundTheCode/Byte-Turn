using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoundTheCode.ByteTurn.Exception
{
    using System.Runtime.Serialization;
using Exception = System.Exception;

    /// <summary>
    /// Thrown if there is an issue with uploading a file.
    /// </summary>
    public partial class ByteTurnUploadFileException : Exception
    {
        public ByteTurnUploadFileException() : base() { }

        public ByteTurnUploadFileException(string message) : base(message) { }

        public ByteTurnUploadFileException(string message, Exception innerException) : base(message, innerException) { }

        public ByteTurnUploadFileException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
