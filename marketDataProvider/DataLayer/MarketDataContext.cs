using System;
using Microsoft.EntityFrameworkCore;
using DataLayer.Models;

namespace DataLayer
{
    public class MarketDataContext : DbContext
    {
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<Exchange> Exchanges { get; set; }
        public DbSet<Market> Markets { get; set; }
        public DbSet<ExchangeMarket> ExchangeMarkets { get; set; }
        public DbSet<ExchangeCurrency> ExchangeCurrencies { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ExchangeMarket>()
                .HasAlternateKey(t => new { t.ExchangeId, t.MarketId });

            modelBuilder.Entity<ExchangeCurrency>()
                .HasAlternateKey(t => new { t.ExchangeId, t.CurrencyId });
        }
        public MarketDataContext(DbContextOptions<MarketDataContext> options) : base(options)
        {
        }
    }
}
