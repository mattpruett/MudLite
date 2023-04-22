using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MattPruett.MUDLite.Data.DataModel.Models
{
    [Table("Tbl_Creatures")]
    public class Creature
    {
        [Column("CR_Key", TypeName = "INTEGER")]
        [Key]
        public int Key { get; set; }

        [Column("CR_Name", TypeName = "VARCHAR")]
        public string Name { get; set; }

        [Column("CR_Description", TypeName = "VARCHAR")]
        public string Description { get; set; }

        [Column("CR_Health", TypeName = "INTEGER")]
        public int Health { get; set; }

        [Column("CR_Attack", TypeName = "INTEGER")]
        public int Attack { get; set; }

        [Column("CR_Defense", TypeName = "INTEGER")]
        public int Defense { get; set; }

        [Column("CR_Evasion", TypeName = "INTEGER")]
        public int Evasion { get; set; }
    }
}
