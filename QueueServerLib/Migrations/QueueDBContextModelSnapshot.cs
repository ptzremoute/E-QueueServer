﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using QueueServerLib;
using System;

namespace QueueServerLib.Migrations
{
    [DbContext(typeof(QueueDBContext))]
    partial class QueueDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125");

            modelBuilder.Entity("QueueLib.OperatorInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("OperatorFIO");

                    b.HasKey("Id");

                    b.ToTable("OperatorInfo");
                });

            modelBuilder.Entity("QueueLib.QueueClientInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ClientNumber");

                    b.Property<DateTime?>("CompleteServiceTime");

                    b.Property<DateTime?>("DequeueTime");

                    b.Property<DateTime>("EnqueueTime");

                    b.Property<int?>("OperatorInfoId");

                    b.Property<int?>("ServiceInfoId");

                    b.Property<int>("WindowNumber");

                    b.HasKey("Id");

                    b.HasIndex("OperatorInfoId");

                    b.HasIndex("ServiceInfoId");

                    b.ToTable("QueueClientInfo");
                });

            modelBuilder.Entity("QueueLib.ServiceInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ServiceName");

                    b.HasKey("Id");

                    b.ToTable("ServiceInfo");
                });

            modelBuilder.Entity("QueueLib.QueueClientInfo", b =>
                {
                    b.HasOne("QueueLib.OperatorInfo", "OperatorInfo")
                        .WithMany("QueueClientInfos")
                        .HasForeignKey("OperatorInfoId");

                    b.HasOne("QueueLib.ServiceInfo", "ServiceInfo")
                        .WithMany("QueueClientInfos")
                        .HasForeignKey("ServiceInfoId");
                });
#pragma warning restore 612, 618
        }
    }
}
