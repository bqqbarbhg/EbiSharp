using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EbiSharp
{
    internal class CompilerException : Exception
    {
        public Location Location;

        public CompilerException(Location location, string message)
            : base(message)
        {
            Location = location;
        }
    }
}
