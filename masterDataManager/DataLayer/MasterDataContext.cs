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
        public DbSet<ExchangeMarket> ExchangeMarkets { get; set; }
        public DbSet<ExchangeCurrency> ExchangeCurrencies { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Exchange-Market OneToMany
            modelBuilder.Entity<ExchangeMarket>()
                .HasAlternateKey(t => new { t.ExchangeId, t.MarketId });

            modelBuilder.Entity<ExchangeMarket>()
                .HasOne(pt => pt.Market)
                .WithMany()
                .HasForeignKey(pt => pt.MarketId);

            modelBuilder.Entity<ExchangeMarket>()
                .HasOne(em => em.Exchange)
                .WithMany(e => e.ExchangeMarkets)
                .HasForeignKey(em => em.ExchangeId);

            // Exchange-Currency OneToMany
            modelBuilder.Entity<ExchangeCurrency>()
                .HasAlternateKey(t => new { t.ExchangeId, t.CurrencyId });

            modelBuilder.Entity<ExchangeCurrency>()
                .HasOne(pt => pt.Currency)
                .WithMany()
                .HasForeignKey(pt => pt.CurrencyId);

            modelBuilder.Entity<ExchangeCurrency>()
                .HasOne(ec => ec.Exchange)
                .WithMany(e => e.ExchangeCurrencies)
                .HasForeignKey(ec => ec.ExchangeId);

            //modelBuilder.Entity<UserAsset>()
              //  .HasAlternateKey(t => new { t.CurrencyId, t.TradingMode });  //ADD LATER!!!

        }
        public MasterDataContext(DbContextOptions<MasterDataContext> options) : base(options)
        {
        }
    }
}
