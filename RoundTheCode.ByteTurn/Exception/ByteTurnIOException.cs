using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoundTheCode.ByteTurn.Exception
{
    using System.Runtime.Serialization;
using Exception = System.Exception;

    /// <summary>
    /// Thrown if an input/output exception has happened.
    /// </summary>
    public partial class ByteTurnIOException : ByteTurnException
    {
        public ByteTurnIOException() : base() { }

        public ByteTurnIOException(string message) : base(message) { }

        public ByteTurnIOException(string message, Exception innerException) : base(message, innerException) { }

        public ByteTurnIOException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
