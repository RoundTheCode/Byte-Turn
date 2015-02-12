using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoundTheCode.ByteTurn.Exception
{
    using System.Runtime.Serialization;
using Exception = System.Exception;

    /// <summary>
    /// Thrown if a file or directory is not supported.
    /// </summary>
    public partial class ByteTurnNotSupportedException : ByteTurnException
    {
        public ByteTurnNotSupportedException() : base() { }

        public ByteTurnNotSupportedException(string message) : base(message) { }

        public ByteTurnNotSupportedException(string message, Exception innerException) : base(message, innerException) { }

        public ByteTurnNotSupportedException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
