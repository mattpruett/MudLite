using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MattPruett.MUDLite.Libraries
{
    internal static class Crypto
    {
        internal static string HashString(string value)
        {
            return string.IsNullOrEmpty(value)
                ? value
                : Globals.Hasher.Hash(value);
        }
    }
}
