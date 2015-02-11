using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoundTheCode.ByteTurn.Exception
{
    using System.Runtime.Serialization;
using Exception = System.Exception;

    /// <summary>
    /// Thrown if the application is not able to access the file.
    /// </summary>
    public partial class ByteTurnUnauthorisedAccessException : ByteTurnException
    {
        public ByteTurnUnauthorisedAccessException() : base() { }

        public ByteTurnUnauthorisedAccessException(string message) : base(message) { }

        public ByteTurnUnauthorisedAccessException(string message, Exception innerException) : base(message, innerException) { }

        public ByteTurnUnauthorisedAccessException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
