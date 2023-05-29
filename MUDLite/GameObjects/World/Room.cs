using MattPruett.MUDLite.Data;
using MattPruett.MUDLite.Data.DataModel.Models;
using MattPruett.MUDLite.GameObjects.Base;
using System.Collections.Generic;
using System.Linq;

namespace MattPruett.MUDLite.GameObjects.World
{
    internal class Room: NamedObject
    {
        public Room (Tbl_Room room) : base(room.Name, room.Description)
        {
            Key = room.Key;
            Exits = room.Exits;
        }

        public Room(string name) : base(name) { }
        public Room(string name, string description) : base(name, description) { }

        public int Key { get; private set; }

        public Dictionary<string, RoomExit> Exits { get; set; }

        public static Room GetRoom(int key)
        {
            using (var db = new MUDLiteDataContext())
            {
                var room = (from rm in db.Rooms
                        where rm.Key == key
                        select rm).FirstOrDefault();

                return room == null ? null : new Room(room);
            }

        }

        public override string ToString()
        {
            var exits = Exits == null || Exits.Count == 0
                ? string.Empty
                : $"\r\nExits: {string.Join(", ", Exits.Keys)}";


            return $"{Description}{exits}";
        }
    }
}
