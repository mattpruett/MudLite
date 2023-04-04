using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MattPruett.MUDLite.GameObjects.Base
{
    internal class Creature : NamedObject
    {
        public Creature(string name) : base(name) { }
        public Creature(string name, string description) : base(name, description) { }

        private int _health = 0;
        private int _attack = 0;
        private int _defense = 0;

        public int Health
        {
            get
            {
                return _health;
            }
            set
            {
                _health = Math.Max(0, value);
            }
        }

        public int Attack
        {
            get
            {
                return _attack;
            }
            set
            {
                _attack = Math.Max(0, value);
            }
        }

        public int Defense
        {
            get
            {
                return _defense;
            }
            set
            {
                _defense = Math.Max(0, value);
            }
        }

        public List<NamedObject> Inventory { get; set; }
    }
}
