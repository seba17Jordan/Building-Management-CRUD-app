using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomExceptions
{
    public class EmptyFieldException : Exception
    {
        public EmptyFieldException() { }

        public EmptyFieldException(string message) : base(message) { }

        public EmptyFieldException(string message, Exception innerException) : base(message, innerException) { }
    }
}
