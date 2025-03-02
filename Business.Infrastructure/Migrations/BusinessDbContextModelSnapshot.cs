﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NodaTime;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using WireOps.Business.Infrastructure.Database.SQL.EntityFramework;

#nullable disable

namespace Business.Infrastructure.Migrations
{
    [DbContext(typeof(BusinessDbContext))]
    partial class BusinessDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Business.Infrastructure.Database.SQL.EntityFramework.Objects.DbCompany", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<Instant>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Instant>("ModifiedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Version")
                        .IsConcurrencyToken()
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Company", (string)null);
                });

            modelBuilder.Entity("Business.Infrastructure.Database.SQL.EntityFramework.Objects.DbRole", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<Guid>("CompanyId")
                        .HasColumnType("uuid");

                    b.Property<Instant>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsOwnerRole")
                        .HasColumnType("boolean");

                    b.Property<Instant>("ModifiedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Version")
                        .IsConcurrencyToken()
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.ToTable("Role", (string)null);
                });

            modelBuilder.Entity("Business.Infrastructure.Database.SQL.EntityFramework.Objects.DbStaffer", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<Guid>("CompanyId")
                        .HasColumnType("uuid");

                    b.Property<Instant>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FamilyName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("GivenName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsOwner")
                        .HasColumnType("boolean");

                    b.Property<Instant>("ModifiedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("RoleId")
                        .HasColumnType("uuid");

                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<int>("Version")
                        .IsConcurrencyToken()
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("RoleId");

                    b.ToTable("Staffer", (string)null);
                });

            modelBuilder.Entity("Business.Infrastructure.Database.SQL.EntityFramework.Objects.DbCompany", b =>
                {
                    b.OwnsOne("WireOps.Business.Domain.Common.ValueObjects.Types.Address", "Address", b1 =>
                        {
                            b1.Property<Guid>("CompanyId")
                                .HasColumnType("uuid");

                            b1.Property<string>("Address1")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<string>("Address2")
                                .HasColumnType("text");

                            b1.Property<string>("City")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<string>("Country")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<string>("PostalCode")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<string>("StateProvince")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.HasKey("CompanyId");

                            b1.ToTable("Company");

                            b1.WithOwner()
                                .HasForeignKey("CompanyId");
                        });

                    b.Navigation("Address");
                });

            modelBuilder.Entity("Business.Infrastructure.Database.SQL.EntityFramework.Objects.DbRole", b =>
                {
                    b.HasOne("Business.Infrastructure.Database.SQL.EntityFramework.Objects.DbCompany", null)
                        .WithMany()
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsMany("WireOps.Business.Domain.Roles.Role+Permission", "Permissions", b1 =>
                        {
                            b1.Property<Guid>("RoleId")
                                .HasColumnType("uuid");

                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("integer");

                            NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b1.Property<int>("Id"));

                            b1.Property<string>("Action")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<string>("Resource")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.HasKey("RoleId", "Id");

                            b1.ToTable("RolePermission", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("RoleId");
                        });

                    b.Navigation("Permissions");
                });

            modelBuilder.Entity("Business.Infrastructure.Database.SQL.EntityFramework.Objects.DbStaffer", b =>
                {
                    b.HasOne("Business.Infrastructure.Database.SQL.EntityFramework.Objects.DbCompany", null)
                        .WithMany()
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Business.Infrastructure.Database.SQL.EntityFramework.Objects.DbRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId");
                });
#pragma warning restore 612, 618
        }
    }
}
