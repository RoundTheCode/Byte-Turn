using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoundTheCode.ByteTurn.Exception
{
    using System.Runtime.Serialization;
using Exception = System.Exception;

    /// <summary>
    /// Thrown if a file or directory path is too long.
    /// </summary>
    public partial class ByteTurnPathTooLongException : ByteTurnException
    {
        public ByteTurnPathTooLongException() : base() { }

        public ByteTurnPathTooLongException(string message) : base(message) { }

        public ByteTurnPathTooLongException(string message, Exception innerException) : base(message, innerException) { }

        public ByteTurnPathTooLongException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
