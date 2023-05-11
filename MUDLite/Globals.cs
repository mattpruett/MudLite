using MattPruett.MUDLite.Data;
using MattPruett.MUDLite.Libraries;
using MattPruett.MUDLite.System;
using System.Collections.Generic;
using System.Linq;

namespace MattPruett.MUDLite
{
    public static class Globals
    {
        internal static Server Server { get; set; }

        internal static StringHasher Hasher { get; set; } = new StringHasher();

        internal static Dictionary<int, Data.DataModel.Models.Room> WorldMap;

        internal static void LoadWorld()
        {
            using (var db = new MUDLiteDataContext()) 
            {
                WorldMap = (from room in db.Rooms
                        select room)
                        .ToDictionary(room => room.Key, room => room);
            }
        }
    }
}
