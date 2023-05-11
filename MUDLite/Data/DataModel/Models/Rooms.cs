using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;

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

        [NotMapped]
        public Dictionary<string, RoomExit> Exits
        {
            get
            {
                using (var db = new MUDLiteDataContext())
                {
                    return (
                        from exit in db.RoomExits
                        where exit.From == Key
                        orderby exit.Name
                        select exit
                    )
                    .ToDictionary(exit => exit.Name, exit => exit);
                }
            }
        }
    }



    [Table("Tbl_RoomExits")]
    public class RoomExit
    {
        [Column("EX_Key", TypeName = "INTEGER")]
        [Key]
        public int Key { get; set; }

        [Column("EX_Name", TypeName = "VARCHAR")]
        public string Name { get; set; }

        [Column("EX_From_RM_Key", TypeName = "INTEGER")]
        public int From { get; set; }

        [Column("EX_To_RM_Key", TypeName = "INTEGER")]
        public int To { get; set; }
    }
}
