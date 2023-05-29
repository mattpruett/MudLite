using MattPruett.MUDLite.Data.DataModel.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MattPruett.MUDLite.GameObjects.Base
{
    internal class Creature : NamedObject
    {
        public Creature(string name) : base(name) { }
        public Creature(string name, string description) : base(name, description) { }

        public Creature(Tbl_Creature creature) : base(creature.Name, creature.Description)
        {
            _health = creature.Health;
            _attack = creature.Attack;
            _defense = creature.Defense;
            _key = creature.Key;
            _evasion = creature.Evasion;
            _rm_key = creature.RM_Key;
        }

        private int _health = 0;
        private int _attack = 0;
        private int _defense = 0;
        private int _key = 0;
        private int _evasion = 0;
        private int? _rm_key;

        public int Key { get { return _key; } }

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

        public int Evasion
        {
            get
            {
                return _evasion;
            }
            set
            {
                _evasion = Math.Max(0, value);
            }
        }

        public int? RM_Key
        {
            get
            {
                return _rm_key;
            }
            set
            {
                _rm_key = value;
            }
        }

        public List<NamedObject> Inventory { get; set; }
    }
}
