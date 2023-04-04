using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MattPruett.MUDLite.GameObjects.Base
{
    internal abstract class NamedObject
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string[] ShortNames { get; set; }

        public NamedObject(string name)
        {
            Name = name;
        }

        public NamedObject(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}