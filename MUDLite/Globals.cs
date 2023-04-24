using MattPruett.MUDLite.Libraries;
using MattPruett.MUDLite.System;

namespace MattPruett.MUDLite
{
    public static class Globals
    {
        internal static Server Server { get; set; }

        internal static StringHasher Hasher { get; set; } = new StringHasher();
    }
}
