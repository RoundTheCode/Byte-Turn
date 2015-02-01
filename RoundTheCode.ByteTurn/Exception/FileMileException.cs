using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoundTheCode.ByteTurn.Exception
{
    using System.Runtime.Serialization;
using Exception = System.Exception;

    public partial class FileMileException : Exception
    {
        public FileMileException() : base() { }

        public FileMileException(string message) : base(message) { }

        public FileMileException(string message, Exception innerException) : base(message, innerException) { }

        public FileMileException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
