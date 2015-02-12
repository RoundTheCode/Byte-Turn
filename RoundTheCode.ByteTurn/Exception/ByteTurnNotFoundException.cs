using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoundTheCode.ByteTurn.Exception
{
    using System.Runtime.Serialization;
using Exception = System.Exception;

    /// <summary>
    /// Thrown if a file or directory cannot be found and prevents the function returning.
    /// </summary>
    public partial class ByteTurnNotFoundException : ByteTurnException
    {
        public ByteTurnNotFoundException() : base() { }

        public ByteTurnNotFoundException(string message) : base(message) { }

        public ByteTurnNotFoundException(string message, Exception innerException) : base(message, innerException) { }

        public ByteTurnNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
