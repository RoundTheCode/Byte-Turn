using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoundTheCode.ByteTurn.Exception
{
    using System.Runtime.Serialization;
using Exception = System.Exception;

    /// <summary>
    /// Thrown if a file or a directory exists which prevents a function being performed.
    /// </summary>
    public partial class ByteTurnExistsException : ByteTurnException
    {
        public ByteTurnExistsException() : base() { }

        public ByteTurnExistsException(string message) : base(message) { }

        public ByteTurnExistsException(string message, Exception innerException) : base(message, innerException) { }

        public ByteTurnExistsException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
