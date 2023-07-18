using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MattPruett.MUDLite.Data.DataModel.Models
{
    [Table("Tbl_PlayerCharacters")]
    public class Tbl_PlayerCharacter
    {
        [Column("PC_Key", TypeName = "INTEGER")]
        [Key]
        public int PC_Key { get; set; }

        [Column("PC_CR_Key", TypeName = "INTEGER")]
        public int PC_CR_Key { get; set; }

        [Column("PC_US_Id", TypeName = "VARCHAR")]
        public string PC_US_Id { get; set; }

        [Column("PC_RL_Key", TypeName = "INTEGER")]
        public int PC_RL_Key { get; set; }
    }
}
