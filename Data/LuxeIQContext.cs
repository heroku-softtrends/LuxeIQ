using Microsoft.EntityFrameworkCore;
using LuxeIQ.Models;
using Humanizer.Localisation;


namespace LuxeIQ.Data
{
    public class LuxeIQContext : DbContext
    {
        public LuxeIQContext(DbContextOptions<LuxeIQContext> options) : base(options)
        {
        }

        public DbSet<Manufacturers> Manufacturers { get; set; }
        public DbSet<ManufacturerTerritories> ManufacturerTerritories { get; set; }
        public DbSet<Wholesalers> Wholesalers { get; set; }
        public DbSet<WholesalerShowrooms> WholesalerShowrooms { get; set; }
        public DbSet<SalesRepAgency> SalesRepAgency { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<WholesalerHQ> WholesalerHQ { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Assign default schema
            modelBuilder.HasDefaultSchema("public");
            modelBuilder.Entity<Manufacturers>().Property(p => p.manufacturerId).HasColumnType("bigint");
            modelBuilder.Entity<Manufacturers>(entity => { entity.Property(e => e.updated_at).HasDefaultValueSql("now()"); });
            modelBuilder.Entity<Manufacturers>(entity => { entity.Property(e => e.created_at).HasDefaultValueSql("now()"); });

            modelBuilder.Entity<ManufacturerTerritories>().Property(p => p.manufacturerId).HasColumnType("bigint");
            modelBuilder.Entity<ManufacturerTerritories>().Property(p => p.territoryId).HasColumnType("bigint");
            modelBuilder.Entity<ManufacturerTerritories>(entity => { entity.Property(e => e.updated_at).HasDefaultValueSql("now()"); });
            modelBuilder.Entity<ManufacturerTerritories>(entity => { entity.Property(e => e.created_at).HasDefaultValueSql("now()"); });

            modelBuilder.Entity<Wholesalers>().Property(p => p.manufacturerId).HasColumnType("bigint");
            modelBuilder.Entity<Wholesalers>().Property(p => p.wholesalerId).HasColumnType("bigint");
            modelBuilder.Entity<Wholesalers>(entity => { entity.Property(e => e.updated_at).HasDefaultValueSql("now()"); });
            modelBuilder.Entity<Wholesalers>(entity => { entity.Property(e => e.created_at).HasDefaultValueSql("now()"); });

            modelBuilder.Entity<WholesalerShowrooms>().Property(p => p.wholesalerId).HasColumnType("bigint");
            modelBuilder.Entity<WholesalerShowrooms>().Property(p => p.showroomId).HasColumnType("bigint");
            modelBuilder.Entity<WholesalerShowrooms>(entity => { entity.Property(e => e.updated_at).HasDefaultValueSql("now()"); });
            modelBuilder.Entity<WholesalerShowrooms>(entity => { entity.Property(e => e.created_at).HasDefaultValueSql("now()"); });

            modelBuilder.Entity<SalesRepAgency>().Property(p => p.salesRepAgencyId).HasColumnType("bigint");
            modelBuilder.Entity<SalesRepAgency>(entity => { entity.Property(e => e.updated_at).HasDefaultValueSql("now()"); });
            modelBuilder.Entity<SalesRepAgency>(entity => { entity.Property(e => e.created_at).HasDefaultValueSql("now()"); });

            modelBuilder.Entity<Users>().Property(p => p.userId).HasColumnType("bigint");
            modelBuilder.Entity<Users>(entity => { entity.Property(e => e.updated_at).HasDefaultValueSql("now()"); });
            modelBuilder.Entity<Users>(entity => { entity.Property(e => e.created_at).HasDefaultValueSql("now()"); });

            modelBuilder.Entity<Products>().Property(p => p.productId).HasColumnType("bigint");
            modelBuilder.Entity<Products>().Property(p => p.manufacturerId).HasColumnType("bigint");
            modelBuilder.Entity<Products>(entity => { entity.Property(e => e.updated_at).HasDefaultValueSql("now()"); });
            modelBuilder.Entity<Products>(entity => { entity.Property(e => e.created_at).HasDefaultValueSql("now()"); });

            modelBuilder.Entity<WholesalerHQ>().Property(p => p.wholesalerHQId).HasColumnType("bigint");
            modelBuilder.Entity<WholesalerHQ>().Property(p => p.wholesalerId).HasColumnType("bigint");
            modelBuilder.Entity<WholesalerHQ>(entity => { entity.Property(e => e.updated_at).HasDefaultValueSql("now()"); });
            modelBuilder.Entity<WholesalerHQ>(entity => { entity.Property(e => e.created_at).HasDefaultValueSql("now()"); });

            base.OnModelCreating(modelBuilder);
        }
    }
}
