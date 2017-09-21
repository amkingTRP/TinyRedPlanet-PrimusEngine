using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyRedPlanet.PrimusEngine
{
    class HelperFunctions
    {
        public static int GetUpper(int original)
        {
            return (original >> 8) & 0xFF;
        }

        public static int GetLower(int original)
        {
            return original & 0xff;
        }

        public static int Combine(int upper, int lower)
        {
            return (upper << 8) | (lower&0xff);
        }
    }
}
