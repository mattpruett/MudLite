using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MattPruett.MUDLite.Data.DataModel.Models
{
    [Table("Tbl_Rooms")]
    public class Room
    {
        [Column("RM_Key", TypeName = "INTEGER")]
        [Key]
        public int Key { get; set; }

        [Column("RM_Name", TypeName = "VARCHAR")]
        public string Name { get; set; }

        [Column("RM_Description", TypeName = "VARCHAR")]
        public string Description { get; set; }
    }
}
