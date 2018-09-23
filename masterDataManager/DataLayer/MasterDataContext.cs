using System;
using Microsoft.EntityFrameworkCore;
using DataLayer.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Collections.Generic;

namespace DataLayer
{
    public class MasterDataContext : IdentityDbContext<User> 
    {
        public DbSet<Strategy> Strategies { get; set; }
        public DbSet<Trade> Trades { get; set; }
        public DbSet<UserAsset> UserAssets { get; set; }
        public DbSet<StrategyAsset> StrategyAssets { get; set; }
        public DbSet<ExchangeSecret> ExchangeSecrets { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ExchangeSecret>()
                .HasAlternateKey(t => new { t.ExchangeId, t.UserId });

            modelBuilder.Entity<UserAsset>()
              .HasAlternateKey(t => new { t.Currency, t.TradingMode, t.Exchange });
        }
        public MasterDataContext(DbContextOptions<MasterDataContext> options) : base(options)
        {
        }
    }
}
