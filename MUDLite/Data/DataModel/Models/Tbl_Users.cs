using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using MattPruett.MUDLite.Libraries;
using MattPruett.MUDLite.System;
using System.ComponentModel;

namespace MattPruett.MUDLite.Data.DataModel.Models
{
    [Table("Tbl_Users")]
    public class Tbl_User
    {
        [Column("US_Id", TypeName = "VARCHAR")]
        [Key]
        public string Id { get; set; }

        [Column("US_Name", TypeName = "VARCHAR")]
        public string Name { get; set; }

        [Column("US_HashedPassword", TypeName = "VARCHAR")]
        public string Password { get; set; }

        [NotMapped]
        public bool HasCharacters
        {
            get
            {
                using (var db = new MUDLiteDataContext())
                {
                    return (
                        from chars in db.Creatures
                        join pcs in db.PlayerCharacters on chars.Key equals pcs.PC_CR_Key
                        where pcs.PC_US_Id == Id
                        select true
                    ).Any();
                }
            }
        }

        [NotMapped]
        public Tbl_Creature Character { get; set; }

        [NotMapped]
        public List<Tbl_Creature> Characters
        {
            get
            {
                using (var db = new MUDLiteDataContext())
                {
                    return (
                        from chars in db.Creatures
                        join pcs in db.PlayerCharacters on chars.Key equals pcs.PC_CR_Key
                        where pcs.PC_US_Id == Id
                        orderby chars.Name
                        select chars
                    ).ToList();
                }
            }
        }

        public Tbl_Creature CharacterByName(string name)
        {
            using (var db = new MUDLiteDataContext())
            {
                return (
                    from chars in db.Creatures
                    join pcs in db.PlayerCharacters on chars.Key equals pcs.PC_CR_Key
                    where pcs.PC_US_Id == Id && chars.Name == name
                    select chars
                ).FirstOrDefault();
            }
        }


        public void CreateCharacter(string name)
        {
            using(var db = new MUDLiteDataContext())
            {
                var creature = new Tbl_Creature
                {
                    Name = name,
                    Attack = 1,
                    Defense = 1,
                    Description = "This character is a total n00b.",
                    Evasion = 1,
                    Health = 10,
                    RM_Key = 1
                };
                db.Creatures.Add(creature);
                db.SaveChanges();

                db.PlayerCharacters.Add(new Tbl_PlayerCharacter { PC_CR_Key = creature.Key, PC_US_Id = Id });
                db.SaveChanges();

                this.Character = creature;
            }
        }
    }
}
