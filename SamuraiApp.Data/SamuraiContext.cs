﻿using Microsoft.EntityFrameworkCore;
using SamuraiApp.Domain;
using System;

namespace SamuraiApp.Data
{
    public class SamuraiContext:DbContext
    {
        public DbSet<Samurai> Samurais { get; set; }
        public DbSet<Quote> Quotes { get; set; }
        public DbSet<Battle> Battles { get; set; }
        public DbSet<SamuraiBattleStat> SamuraiBattleStats { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                "Data Source= (localdb)\\MSSQLLocalDB; Initial Catalog=SamuraiAppData"//,
                                                                                      //options=>options.MaxBatchSize=100)
                ).LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name })
                //LogTo(Console.WriteLine,new[] { DbLoggerCategory.Database.Command.Name,
                //                                  DbLoggerCategory.Database.Transaction.Name },
                //        LogLevel.Debug)
                
                .EnableSensitiveDataLogging();
            //base.OnConfiguring(optionsBuilder);
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
