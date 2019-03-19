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
        //public DbSet<EvaluationTick> Evaluations{ get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ExchangeSecret>()
                .HasAlternateKey(t => new { t.ExchangeId, t.UserId });

            builder.Entity<UserAsset>()
              .HasAlternateKey(t => new { t.Currency, t.TradingMode, t.Exchange });
        }
        public MasterDataContext(DbContextOptions<MasterDataContext> options) : base(options)
        {
        }
    }
}
