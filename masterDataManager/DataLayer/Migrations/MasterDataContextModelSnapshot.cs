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

            modelBuilder.Entity("DataLayer.Models.EvaluationTick", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("BtcValue");

                    b.Property<string>("StrategyId");

                    b.Property<DateTime>("TimeStamp");

                    b.Property<decimal>("UsdValue");

                    b.HasKey("Id");

                    b.HasIndex("StrategyId");

                    b.ToTable("EvaluationTick");
                });

            modelBuilder.Entity("DataLayer.Models.ExchangeSecret", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ApiKey");

                    b.Property<string>("ApiSecret");

                    b.Property<string>("ExchangeId")
                        .IsRequired();

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasAlternateKey("ExchangeId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("ExchangeSecrets");
                });

            modelBuilder.Entity("DataLayer.Models.Strategy", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("ExchangeId");

                    b.Property<string>("Name");

                    b.Property<int>("NewTrades");

                    b.Property<DateTime>("Start");

                    b.Property<DateTime>("Stop");

                    b.Property<int>("StrategyState");

                    b.Property<int>("TradingMode");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Strategies");
                });

            modelBuilder.Entity("DataLayer.Models.StrategyAsset", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("Amount");

                    b.Property<string>("StrategyId");

                    b.Property<string>("UserAssetId");

                    b.HasKey("Id");

                    b.HasIndex("StrategyId");

                    b.HasIndex("UserAssetId");

                    b.ToTable("StrategyAssets");
                });

            modelBuilder.Entity("DataLayer.Models.Trade", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("Closed");

                    b.Property<string>("ExchangeId");

                    b.Property<string>("ExchangeUuid");

                    b.Property<string>("MarketId");

                    b.Property<DateTime>("Opened");

                    b.Property<int>("OrderType");

                    b.Property<double>("Price");

                    b.Property<double>("Quantity");

                    b.Property<double>("QuantityRemaining");

                    b.Property<string>("StrategyId");

                    b.Property<int>("TradeState");

                    b.HasKey("Id");

                    b.HasIndex("StrategyId");

                    b.ToTable("Trades");
                });

            modelBuilder.Entity("DataLayer.Models.User", b =>
                {
                    b.Property<string>("Id")
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
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("Amount");

                    b.Property<string>("Currency")
                        .IsRequired();

                    b.Property<string>("Exchange")
                        .IsRequired();

                    b.Property<int>("TradingMode");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasAlternateKey("Currency", "TradingMode", "Exchange");

                    b.HasIndex("UserId");

                    b.ToTable("UserAssets");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
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

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("DataLayer.Models.EvaluationTick", b =>
                {
                    b.HasOne("DataLayer.Models.Strategy")
                        .WithMany("Evaluations")
                        .HasForeignKey("StrategyId");
                });

            modelBuilder.Entity("DataLayer.Models.ExchangeSecret", b =>
                {
                    b.HasOne("DataLayer.Models.User", "User")
                        .WithMany("ExchangeSecrets")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DataLayer.Models.Strategy", b =>
                {
                    b.HasOne("DataLayer.Models.User", "User")
                        .WithMany("Strategies")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("DataLayer.Models.StrategyAsset", b =>
                {
                    b.HasOne("DataLayer.Models.Strategy", "Strategy")
                        .WithMany("StrategyAssets")
                        .HasForeignKey("StrategyId");

                    b.HasOne("DataLayer.Models.UserAsset", "UserAsset")
                        .WithMany("StrategyAssets")
                        .HasForeignKey("UserAssetId");
                });

            modelBuilder.Entity("DataLayer.Models.Trade", b =>
                {
                    b.HasOne("DataLayer.Models.Strategy", "Strategy")
                        .WithMany("Trades")
                        .HasForeignKey("StrategyId");
                });

            modelBuilder.Entity("DataLayer.Models.UserAsset", b =>
                {
                    b.HasOne("DataLayer.Models.User")
                        .WithMany("UserAssets")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("DataLayer.Models.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("DataLayer.Models.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DataLayer.Models.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
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
