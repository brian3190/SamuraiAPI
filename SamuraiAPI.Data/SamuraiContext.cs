﻿using Microsoft.EntityFrameworkCore;
using SamuraiAPI.Domain;

namespace SamuraiAPI.Data
{
    public class SamuraiContext : DbContext
    {
        public SamuraiContext()
        { }
        public SamuraiContext(DbContextOptions options)
            : base(options)
        { }
        public DbSet<Samurai> Samurais { get; set; }
        public DbSet<Quote> Quotes { get; set; }
        public DbSet<Battle> Battles { get; set; }
        public DbSet<SamuraiBattleStat> SamuraiBattleStats { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
                    "Data Source= (localdb)\\MSSQLLocalDB; Initial Catalog=SamuraiTestData");
            }
            //base.onconfiguring(optionsbuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Samurai>()
                .HasMany(s => s.Battles)
                .WithMany(b => b.Samurais)
                .UsingEntity<BattleSamurai>
                  (bs => bs.HasOne<Battle>().WithMany(),
                   bs => bs.HasOne<Samurai>().WithMany())
                .Property(bs => bs.DateJoined)
                .HasDefaultValueSql("getdate()");
            modelBuilder.Entity<Horse>().ToTable("Horses");
            modelBuilder.Entity<SamuraiBattleStat>().HasNoKey().ToView("SamuraiBattleStats");//EF Core will never track entities marked with HasNoKey()
        }
    }
}
