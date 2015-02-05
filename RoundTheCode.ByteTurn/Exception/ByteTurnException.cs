using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoundTheCode.ByteTurn.Exception
{
    using System.Runtime.Serialization;
using Exception = System.Exception;

    public partial class ByteTurnException : Exception
    {
        public ByteTurnException() : base() { }

        public ByteTurnException(string message) : base(message) { }

        public ByteTurnException(string message, Exception innerException) : base(message, innerException) { }

        public ByteTurnException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
