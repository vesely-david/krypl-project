﻿// <auto-generated />
using DataLayer;
using DataLayer.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace DataLayer.Migrations
{
    [DbContext(typeof(MasterDataContext))]
    partial class MasterDataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125");

            modelBuilder.Entity("DataLayer.Models.Currency", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Currencies");
                });

            modelBuilder.Entity("DataLayer.Models.EvaluationTick", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("BtcValue");

                    b.Property<int?>("StrategyId");

                    b.Property<DateTime>("TimeStamp");

                    b.Property<double>("UsdValue");

                    b.HasKey("Id");

                    b.HasIndex("StrategyId");

                    b.ToTable("EvaluationTick");
                });

            modelBuilder.Entity("DataLayer.Models.Exchange", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<string>("Web");

                    b.HasKey("Id");

                    b.ToTable("Exchanges");
                });

            modelBuilder.Entity("DataLayer.Models.ExchangeCurrency", b =>
                {
                    b.Property<int>("ExchangeId");

                    b.Property<int>("CurrencyId");

                    b.Property<string>("ExchangeCurrencyCode");

                    b.Property<int>("Id");

                    b.HasKey("ExchangeId", "CurrencyId");

                    b.HasIndex("CurrencyId");

                    b.ToTable("ExchangeCurrencies");
                });

            modelBuilder.Entity("DataLayer.Models.ExchangeMarket", b =>
                {
                    b.Property<int>("ExchangeId");

                    b.Property<int>("MarketId");

                    b.Property<string>("ExchangeMarketName");

                    b.HasKey("ExchangeId", "MarketId");

                    b.HasIndex("MarketId");

                    b.ToTable("ExchangeMarkets");
                });

            modelBuilder.Entity("DataLayer.Models.ExchangeSecret", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ApiKey");

                    b.Property<string>("ApiSecret");

                    b.Property<int>("ExchangeId");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("ExchangeId");

                    b.HasIndex("UserId");

                    b.ToTable("ExchangeSecrets");
                });

            modelBuilder.Entity("DataLayer.Models.Market", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("BaseCurrencyId");

                    b.Property<string>("Code");

                    b.Property<int>("SecondaryCurrencyId");

                    b.HasKey("Id");

                    b.HasIndex("BaseCurrencyId");

                    b.HasIndex("SecondaryCurrencyId");

                    b.ToTable("Markets");
                });

            modelBuilder.Entity("DataLayer.Models.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("DataLayer.Models.Strategy", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<int>("ExchangeId");

                    b.Property<string>("Name");

                    b.Property<int>("NewTrades");

                    b.Property<DateTime>("Start");

                    b.Property<DateTime>("Stop");

                    b.Property<int>("StrategyState");

                    b.Property<int>("TradingMode");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("ExchangeId");

                    b.HasIndex("UserId");

                    b.ToTable("Strategies");
                });

            modelBuilder.Entity("DataLayer.Models.StrategyAsset", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("Amount");

                    b.Property<int>("StrategyId");

                    b.Property<int>("UserAssetId");

                    b.HasKey("Id");

                    b.HasIndex("StrategyId");

                    b.HasIndex("UserAssetId");

                    b.ToTable("StrategyAssets");
                });

            modelBuilder.Entity("DataLayer.Models.Trade", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("Closed");

                    b.Property<int>("ExchangeId");

                    b.Property<string>("ExchangeUuid");

                    b.Property<int>("MarketId");

                    b.Property<DateTime>("Opened");

                    b.Property<int>("OrderType");

                    b.Property<double>("Price");

                    b.Property<double>("Quantity");

                    b.Property<double>("QuantityRemaining");

                    b.Property<int>("StrategyId");

                    b.Property<int>("TradeState");

                    b.HasKey("Id");

                    b.HasIndex("ExchangeId");

                    b.HasIndex("MarketId");

                    b.HasIndex("StrategyId");

                    b.ToTable("Trades");
                });

            modelBuilder.Entity("DataLayer.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("DataLayer.Models.UserAsset", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("Amount");

                    b.Property<int>("CurrencyId");

                    b.Property<int>("ExchangeId");

                    b.Property<int>("TradingMode");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("CurrencyId");

                    b.HasIndex("ExchangeId");

                    b.HasIndex("UserId");

                    b.ToTable("UserAssets");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<int>("RoleId");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<int>("UserId");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<int>", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<int>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("DataLayer.Models.EvaluationTick", b =>
                {
                    b.HasOne("DataLayer.Models.Strategy")
                        .WithMany("Evaluation")
                        .HasForeignKey("StrategyId");
                });

            modelBuilder.Entity("DataLayer.Models.ExchangeCurrency", b =>
                {
                    b.HasOne("DataLayer.Models.Currency", "Currency")
                        .WithMany()
                        .HasForeignKey("CurrencyId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DataLayer.Models.Exchange", "Exchange")
                        .WithMany("ExchangeCurrencies")
                        .HasForeignKey("ExchangeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DataLayer.Models.ExchangeMarket", b =>
                {
                    b.HasOne("DataLayer.Models.Exchange", "Exchange")
                        .WithMany("ExchangeMarkets")
                        .HasForeignKey("ExchangeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DataLayer.Models.Market", "Market")
                        .WithMany()
                        .HasForeignKey("MarketId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DataLayer.Models.ExchangeSecret", b =>
                {
                    b.HasOne("DataLayer.Models.Exchange", "Exchange")
                        .WithMany("ExchangeSecrets")
                        .HasForeignKey("ExchangeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DataLayer.Models.User", "User")
                        .WithMany("ExchangeSecrets")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DataLayer.Models.Market", b =>
                {
                    b.HasOne("DataLayer.Models.Currency", "BaseCurrency")
                        .WithMany()
                        .HasForeignKey("BaseCurrencyId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DataLayer.Models.Currency", "SecondaryCurrency")
                        .WithMany()
                        .HasForeignKey("SecondaryCurrencyId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DataLayer.Models.Strategy", b =>
                {
                    b.HasOne("DataLayer.Models.Exchange", "Exchange")
                        .WithMany()
                        .HasForeignKey("ExchangeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DataLayer.Models.User", "User")
                        .WithMany("Strategies")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DataLayer.Models.StrategyAsset", b =>
                {
                    b.HasOne("DataLayer.Models.Strategy", "Strategy")
                        .WithMany("StrategyAssets")
                        .HasForeignKey("StrategyId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DataLayer.Models.UserAsset", "UserAsset")
                        .WithMany("StrategyAssets")
                        .HasForeignKey("UserAssetId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DataLayer.Models.Trade", b =>
                {
                    b.HasOne("DataLayer.Models.Exchange", "Exchange")
                        .WithMany()
                        .HasForeignKey("ExchangeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DataLayer.Models.Market", "Market")
                        .WithMany()
                        .HasForeignKey("MarketId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DataLayer.Models.Strategy", "Strategy")
                        .WithMany("Trades")
                        .HasForeignKey("StrategyId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DataLayer.Models.UserAsset", b =>
                {
                    b.HasOne("DataLayer.Models.Currency", "Currency")
                        .WithMany()
                        .HasForeignKey("CurrencyId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DataLayer.Models.Exchange", "Exchange")
                        .WithMany()
                        .HasForeignKey("ExchangeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DataLayer.Models.User", "User")
                        .WithMany("UserAssets")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.HasOne("DataLayer.Models.Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.HasOne("DataLayer.Models.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.HasOne("DataLayer.Models.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<int>", b =>
                {
                    b.HasOne("DataLayer.Models.Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DataLayer.Models.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.HasOne("DataLayer.Models.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
