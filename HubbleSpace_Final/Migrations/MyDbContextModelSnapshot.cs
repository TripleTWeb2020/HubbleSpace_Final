﻿// <auto-generated />
using System;
using HubbleSpace_Final.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace HubbleSpace_Final.Migrations
{
    [DbContext(typeof(MyDbContext))]
    partial class MyDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("HubbleSpace_Final.Entities.Account", b =>
                {
                    b.Property<int>("ID_Account")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<int>("level")
                        .HasColumnType("int");

                    b.HasKey("ID_Account");

                    b.ToTable("Account");
                });

            modelBuilder.Entity("HubbleSpace_Final.Entities.Banner", b =>
                {
                    b.Property<int>("ID_Banner")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Banner_Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("Date_Upload")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Photo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID_Banner");

                    b.ToTable("Banner");
                });

            modelBuilder.Entity("HubbleSpace_Final.Entities.Brand", b =>
                {
                    b.Property<int>("ID_Brand")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Brand_Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.HasKey("ID_Brand");

                    b.ToTable("Brand");
                });

            modelBuilder.Entity("HubbleSpace_Final.Entities.Category", b =>
                {
                    b.Property<int>("ID_Categorie")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Category_Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.HasKey("ID_Categorie");

                    b.ToTable("Category");
                });

            modelBuilder.Entity("HubbleSpace_Final.Entities.Client", b =>
                {
                    b.Property<int>("ID_Client")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Client_Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("CreditCard")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DOB")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Gender")
                        .HasColumnType("int");

                    b.Property<int>("ID_Account")
                        .HasColumnType("int");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID_Client");

                    b.HasIndex("ID_Account");

                    b.ToTable("Client");
                });

            modelBuilder.Entity("HubbleSpace_Final.Entities.Color_Product", b =>
                {
                    b.Property<int>("ID_Color_Product")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Color_Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<int>("ID_Product")
                        .HasColumnType("int");

                    b.HasKey("ID_Color_Product");

                    b.HasIndex("ID_Product");

                    b.ToTable("Color_Product");
                });

            modelBuilder.Entity("HubbleSpace_Final.Entities.Discount", b =>
                {
                    b.Property<int>("ID_Discount")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Code_Discount")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("Expire")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("NumberofTurn")
                        .HasColumnType("int");

                    b.Property<int>("Value")
                        .HasColumnType("int");

                    b.HasKey("ID_Discount");

                    b.ToTable("Discount");
                });

            modelBuilder.Entity("HubbleSpace_Final.Entities.Employee", b =>
                {
                    b.Property<int>("ID_Employee")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Citizen_ID")
                        .IsRequired()
                        .HasColumnType("nvarchar(11)")
                        .HasMaxLength(11);

                    b.Property<string>("DOB")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Employee_Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<int>("Gender")
                        .HasColumnType("int");

                    b.Property<int>("ID_Account")
                        .HasColumnType("int");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Salary")
                        .HasColumnType("float");

                    b.HasKey("ID_Employee");

                    b.HasIndex("ID_Account");

                    b.ToTable("Employee");
                });

            modelBuilder.Entity("HubbleSpace_Final.Entities.Img_Product", b =>
                {
                    b.Property<int>("ID_Img_Product")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ID_Color_Product")
                        .HasColumnType("int");

                    b.Property<string>("Photo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ProductID_Product")
                        .HasColumnType("int");

                    b.HasKey("ID_Img_Product");

                    b.HasIndex("ID_Color_Product");

                    b.HasIndex("ProductID_Product");

                    b.ToTable("Img_Product");
                });

            modelBuilder.Entity("HubbleSpace_Final.Entities.Order", b =>
                {
                    b.Property<int>("ID_Order")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Cashier")
                        .HasColumnType("int");

                    b.Property<int>("Client")
                        .HasColumnType("int");

                    b.Property<int>("Date")
                        .HasColumnType("int");

                    b.Property<int?>("ID_Client")
                        .HasColumnType("int");

                    b.Property<int?>("ID_Employee")
                        .HasColumnType("int");

                    b.Property<double>("TotalMoney")
                        .HasColumnType("float");

                    b.HasKey("ID_Order");

                    b.HasIndex("ID_Client");

                    b.HasIndex("ID_Employee");

                    b.ToTable("Order");
                });

            modelBuilder.Entity("HubbleSpace_Final.Entities.OrderDetail", b =>
                {
                    b.Property<int>("ID_OrderDetail")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ID_Order")
                        .HasColumnType("int");

                    b.Property<int>("ID_Product")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("ID_OrderDetail");

                    b.HasIndex("ID_Order");

                    b.HasIndex("ID_Product");

                    b.ToTable("OrderDetail");
                });

            modelBuilder.Entity("HubbleSpace_Final.Entities.Product", b =>
                {
                    b.Property<int>("ID_Product")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Category")
                        .HasColumnType("int");

                    b.Property<int>("ID_Brand")
                        .HasColumnType("int");

                    b.Property<int?>("ID_Categorie")
                        .HasColumnType("int");

                    b.Property<double>("Price_Product")
                        .HasColumnType("float");

                    b.Property<double>("Price_Sale")
                        .HasColumnType("float");

                    b.Property<string>("Product_Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.HasKey("ID_Product");

                    b.HasIndex("ID_Brand");

                    b.HasIndex("ID_Categorie");

                    b.ToTable("Product");
                });

            modelBuilder.Entity("HubbleSpace_Final.Entities.Client", b =>
                {
                    b.HasOne("HubbleSpace_Final.Entities.Account", "Account")
                        .WithMany()
                        .HasForeignKey("ID_Account")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("HubbleSpace_Final.Entities.Color_Product", b =>
                {
                    b.HasOne("HubbleSpace_Final.Entities.Product", "Product")
                        .WithMany("Color_Products")
                        .HasForeignKey("ID_Product")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("HubbleSpace_Final.Entities.Employee", b =>
                {
                    b.HasOne("HubbleSpace_Final.Entities.Account", "Account")
                        .WithMany()
                        .HasForeignKey("ID_Account")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("HubbleSpace_Final.Entities.Img_Product", b =>
                {
                    b.HasOne("HubbleSpace_Final.Entities.Color_Product", "Color_Product")
                        .WithMany("Img_Products")
                        .HasForeignKey("ID_Color_Product")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HubbleSpace_Final.Entities.Product", null)
                        .WithMany("Img_Products")
                        .HasForeignKey("ProductID_Product");
                });

            modelBuilder.Entity("HubbleSpace_Final.Entities.Order", b =>
                {
                    b.HasOne("HubbleSpace_Final.Entities.Client", "client")
                        .WithMany()
                        .HasForeignKey("ID_Client");

                    b.HasOne("HubbleSpace_Final.Entities.Employee", "Employee")
                        .WithMany()
                        .HasForeignKey("ID_Employee");
                });

            modelBuilder.Entity("HubbleSpace_Final.Entities.OrderDetail", b =>
                {
                    b.HasOne("HubbleSpace_Final.Entities.Order", "Order")
                        .WithMany("OrderDetails")
                        .HasForeignKey("ID_Order")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HubbleSpace_Final.Entities.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ID_Product")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("HubbleSpace_Final.Entities.Product", b =>
                {
                    b.HasOne("HubbleSpace_Final.Entities.Brand", "Brand")
                        .WithMany("Products")
                        .HasForeignKey("ID_Brand")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HubbleSpace_Final.Entities.Category", "category")
                        .WithMany("Products")
                        .HasForeignKey("ID_Categorie");
                });
#pragma warning restore 612, 618
        }
    }
}
