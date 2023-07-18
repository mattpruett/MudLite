using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MattPruett.MUDLite.Data.DataModel.Models
{
    [Table("Tbl_Roles")]
    public class Tbl_Role
    {
        [Column("RL_Key", TypeName = "INTEGER")]
        [Key]
        public int RL_Key { get; set; }

        [Column("RL_Name", TypeName = "VARCHAR")]
        public string RL_Name { get; set; }
    }
}
