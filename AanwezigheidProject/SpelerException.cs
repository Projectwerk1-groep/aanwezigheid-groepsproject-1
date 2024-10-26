using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AanwezigheidProject
{
    public class SpelerException : Exception
    {
        public SpelerException(string? message) : base(message)
        {
        }

        public SpelerException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
