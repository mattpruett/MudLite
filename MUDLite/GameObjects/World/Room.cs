using MattPruett.MUDLite.GameObjects.Base;
using System.Collections.Generic;

namespace MattPruett.MUDLite.GameObjects.World
{
    internal class Room: NamedObject
    {
        public Room(string name) : base(name) { }
        public Room(string name, string description) : base(name, description) { }

        public Dictionary<string, Room> Exits { get; set; }
    }
}
