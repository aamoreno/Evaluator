using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluator
{
    public class InvalidFormat : Exception
    {

        public InvalidFormat(string message) 
            : base(message)
        {

        }

        public InvalidFormat(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
}
