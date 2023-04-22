using MattPruett.MUDLite.Libraries;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.SQLite;

namespace MattPruett.MUDLite.Data
{
    public class SQLiteEFTestDataModel : DbContext
    {
        // Your context has been configured to use a 'SQLiteEFTestDataModel' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'SQLiteEFTest.DataModel.SQLiteEFTestDataModel' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'SQLiteEFTestDataModel' 
        // connection string in the application configuration file.
        public SQLiteEFTestDataModel() :
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
    }
}