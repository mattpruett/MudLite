using MattPruett.MUDLite.Libraries;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.SQLite;

namespace MattPruett.MUDLite.Data
{
    public class MUDLiteDataContext : DbContext
    {
        public MUDLiteDataContext() :
            base(new SQLiteConnection()
            {
                ConnectionString = new SQLiteConnectionStringBuilder
                {
                    DataSource = Constants.Database,
                    ForeignKeys = true
                }.ConnectionString
            }, true)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
        }

        // Since there can be class name clash, I will qualify the type with the namespace.
        public virtual DbSet<DataModel.Models.Tbl_Room> Rooms { get; set; }
        public virtual DbSet<DataModel.Models.Tbl_Creature> Creatures { get; set; }
        public virtual DbSet<DataModel.Models.Tbl_User> Users { get; set; }
        public virtual DbSet<DataModel.Models.Tbl_PlayerCharacter> PlayerCharacters { get; set; }
        public virtual DbSet<DataModel.Models.RoomExit> RoomExits { get; set; }

    }
}