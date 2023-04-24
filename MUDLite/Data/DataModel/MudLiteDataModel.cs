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
        public virtual DbSet<DataModel.Models.Room> Rooms { get; set; }
        public virtual DbSet<DataModel.Models.Creature> Creatures { get; set; }
        public virtual DbSet<DataModel.Models.User> Users { get; set; }
    }
}