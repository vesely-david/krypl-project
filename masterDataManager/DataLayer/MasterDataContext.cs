using System;
using Microsoft.EntityFrameworkCore;
using DataLayer.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Collections.Generic;

namespace DataLayer
{
    public class MasterDataContext : IdentityDbContext<User, Role, int> 
    {
        public DbSet<Strategy> Strategies { get; set; }
        public DbSet<Trade> Trades { get; set; }
        public DbSet<UserAsset> UserAssets { get; set; }
        public DbSet<StrategyAsset> StrategyAssets { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<Exchange> Exchanges { get; set; }
        public DbSet<Market> Markets { get; set; }
        public DbSet<ExchangeSecret> ExchangeSecrets { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=develop.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ExchangeMarket>()
                .HasKey(t => new { t.ExchangeId, t.MarketId});

            modelBuilder.Entity<ExchangeMarket>()
                .HasOne(pt => pt.Market)
                .WithMany()
                .HasForeignKey(pt => pt.MarketId);

            modelBuilder.Entity<ExchangeMarket>()
                .HasOne(em => em.Exchange)
                .WithMany(e => e.Markets)
                .HasForeignKey(em => em.ExchangeId);

        }
        public MasterDataContext(DbContextOptions<MasterDataContext> options) : base(options)
        {
        }
    }
}
