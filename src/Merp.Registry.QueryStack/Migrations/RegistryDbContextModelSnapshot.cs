﻿// <auto-generated />
using Merp.Registry.QueryStack;
using Merp.Registry.QueryStack.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace Merp.Registry.QueryStack.Migrations
{
    [DbContext(typeof(RegistryDbContext))]
    partial class RegistryDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Merp.Registry.QueryStack.Model.Company", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AdministrativeContact");

                    b.Property<string>("CompanyName")
                        .IsRequired();

                    b.Property<string>("MainContact");

                    b.Property<string>("NationalIdentificationNumber");

                    b.Property<Guid>("OriginalId");

                    b.Property<string>("VatIndex");

                    b.HasKey("Id");

                    b.HasIndex("OriginalId");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("Merp.Registry.QueryStack.Model.Party", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("DisplayName")
                        .HasMaxLength(200);

                    b.Property<string>("EmailAddress");

                    b.Property<string>("FaxNumber");

                    b.Property<string>("InstantMessaging");

                    b.Property<string>("Linkedin");

                    b.Property<string>("MobileNumber");

                    b.Property<string>("NationalIdentificationNumber");

                    b.Property<Guid>("OriginalId");

                    b.Property<string>("PhoneNumber");

                    b.Property<int>("Type");

                    b.Property<string>("VatIndex");

                    b.Property<string>("WebsiteAddress");

                    b.HasKey("Id");

                    b.HasIndex("DisplayName");

                    b.HasIndex("OriginalId");

                    b.ToTable("Parties");
                });

            modelBuilder.Entity("Merp.Registry.QueryStack.Model.Person", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("DisplayName")
                        .HasMaxLength(200);

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.Property<string>("NationalIdentificationNumber");

                    b.Property<Guid>("OriginalId");

                    b.Property<string>("VatIndex");

                    b.HasKey("Id");

                    b.HasIndex("DisplayName");

                    b.HasIndex("OriginalId");

                    b.ToTable("People");
                });

            modelBuilder.Entity("Merp.Registry.QueryStack.Model.Party", b =>
                {
                    b.OwnsOne("Merp.Registry.QueryStack.Model.PostalAddress", "LegalAddress", b1 =>
                        {
                            b1.Property<int>("PartyId");

                            b1.Property<string>("Address");

                            b1.Property<string>("City");

                            b1.Property<string>("Country");

                            b1.Property<string>("PostalCode");

                            b1.Property<string>("Province");

                            b1.ToTable("Parties");

                            b1.HasOne("Merp.Registry.QueryStack.Model.Party")
                                .WithOne("LegalAddress")
                                .HasForeignKey("Merp.Registry.QueryStack.Model.PostalAddress", "PartyId")
                                .OnDelete(DeleteBehavior.Cascade);
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
