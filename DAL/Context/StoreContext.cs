using System.Configuration;
using System.Reflection;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Context
{
    public class StoreContext : DbContext
    {
        public StoreContext()
        {
        }

        public StoreContext(DbContextOptions<StoreContext> options) : base(options)
        {
        }
        /*   public StoreContext()
          {
          }
   */
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductBrand> ProductBrands { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string ConnString = ConfigurationManager.AppSettings["SkinetConnection"];
            if (string.IsNullOrEmpty(ConnString))
            { ConnString = "Data Source=nabiitnb;Initial Catalog=Skinet;User ID=sa;Password=Yng@2012;persist security info=True; MultipleActiveResultSets=True;"; }
            optionsBuilder.UseSqlServer(ConnString);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}