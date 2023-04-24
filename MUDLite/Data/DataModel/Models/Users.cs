using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MattPruett.MUDLite.Data.DataModel.Models
{
    [Table("Tbl_Users")]
    public class User
    {
        [Column("US_Id", TypeName = "VARCHAR")]
        [Key]
        public string Id { get; set; }

        [Column("US_Name", TypeName = "VARCHAR")]
        public string Name { get; set; }

        [Column("US_HashedPassword", TypeName = "VARCHAR")]
        public string Password { get; set; }
    }
}
