using Microsoft.EntityFrameworkCore;

namespace Lab2_Web.Models;

public class MapAPIContext : DbContext
{
    public MapAPIContext()
    {
    }
    
    public MapAPIContext(DbContextOptions<MapAPIContext> options) : base(options)
    {
        Database.EnsureCreated();
    }
    
     protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Block>(entity =>
            {
                entity.Property(e => e.Id)
                    .UseIdentityColumn();
                entity.Property(e => e.Name)
                    .HasMaxLength(255);
            });            
            modelBuilder.Entity<Continent>(entity =>
            {
                entity.Property(e => e.Id)
                    .UseIdentityColumn();
                entity.Property(e => e.Name)
                    .HasMaxLength(255);
            });            
            modelBuilder.Entity<ContinentCountry>(entity =>
            {
                entity.Property(e => e.Id)
                    .UseIdentityColumn();
                
                entity.HasOne(d => d.Continent)
                    .WithMany(p => p.ContinentCountries)
                    .HasForeignKey(d => d.ContinentId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.ContinentsCountries)
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.Cascade);
            });            
            modelBuilder.Entity<Country>(entity =>
            {
                entity.Property(e => e.Id)
                    .UseIdentityColumn();
                entity.Property(e => e.Name)
                    .HasMaxLength(255);
                entity.Property(e => e.MilitaryStrength);

                entity.HasOne(d => d.Block)
                    .WithMany(p => p.Countries)
                    .HasForeignKey(d => d.BlockId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.Leader)
                    .WithOne(p => p.Country)
                    .OnDelete(DeleteBehavior.Cascade);
            });            
            modelBuilder.Entity<Leader>(entity =>
            {
                entity.Property(e => e.Id)
                    .UseIdentityColumn();
                entity.Property(e => e.Name);
            });
        }
    
    public virtual DbSet<Block> Blocks { get; set; }

    public virtual DbSet<Continent> Continents { get; set; }
    public virtual DbSet<ContinentCountry> ContinentCountries { get; set; }
    
    public virtual DbSet<Country> Countries { get; set; }
    
    public virtual DbSet<Leader> Leaders { get; set; }
}