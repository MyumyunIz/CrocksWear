using BusinessLayer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Reflection.Metadata;

namespace DataLayer
{
    public class CrockDBContext : IdentityDbContext<User>
    {
        public CrockDBContext()
        {

        }
        public CrockDBContext(DbContextOptions contextOptions) : base(contextOptions)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder OptionsBuilder)
        {
            if (!OptionsBuilder.IsConfigured)
            {
                OptionsBuilder.UseSqlServer("Server=DESKTOP-J5Q5EQD;Database=Crockswear;Trusted_Connection=True;TrustServerCertificate=True;");
            }
            base.OnConfiguring(OptionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>().Property(o => o.Status).HasConversion<string>();


            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                // equivalent of modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
                entityType.SetTableName(entityType.DisplayName());

                // equivalent of modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
                entityType.GetForeignKeys()
                    .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade)
                    .ToList()
                    .ForEach(fk => fk.DeleteBehavior = DeleteBehavior.Restrict);
            }


            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Shoe> Shoes { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Bill> Bills { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<Image> Images { get; set; }


    }
}