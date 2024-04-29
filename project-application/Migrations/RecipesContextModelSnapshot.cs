﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Oracle.EntityFrameworkCore.Metadata;

#nullable disable

namespace project.Migrations
{
    [DbContext(typeof(RecipesContext))]
    partial class RecipesContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            OracleModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("RecipeUser", b =>
                {
                    b.Property<int>("UserFavoriteRecipiesId")
                        .HasColumnType("NUMBER(10)");

                    b.Property<int>("UserFavouriteUserId")
                        .HasColumnType("NUMBER(10)");

                    b.HasKey("UserFavoriteRecipiesId", "UserFavouriteUserId");

                    b.HasIndex("UserFavouriteUserId");

                    b.ToTable("RecipeUser");
                });

            modelBuilder.Entity("Tag", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("NUMBER(10)");

                    OraclePropertyBuilderExtensions.UseIdentityColumn(b.Property<int?>("Id"));

                    b.Property<int?>("RecipeId")
                        .HasColumnType("NUMBER(10)");

                    b.Property<string>("TagName")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.HasKey("Id");

                    b.HasIndex("RecipeId");

                    b.ToTable("Tag");
                });

            modelBuilder.Entity("recipes.Ingredient", b =>
                {
                    b.Property<int>("IngredientId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("NUMBER(10)");

                    OraclePropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IngredientId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<int>("Unit")
                        .HasColumnType("NUMBER(10)");

                    b.HasKey("IngredientId");

                    b.ToTable("RecipeManager_Ingredients");
                });

            modelBuilder.Entity("recipes.Instruction", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("NUMBER(10)");

                    OraclePropertyBuilderExtensions.UseIdentityColumn(b.Property<int?>("Id"));

                    b.Property<int>("Index")
                        .HasColumnType("NUMBER(10)");

                    b.Property<int?>("RecipeId")
                        .HasColumnType("NUMBER(10)");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.HasKey("Id");

                    b.HasIndex("RecipeId");

                    b.ToTable("RecipeManager_Instructions");
                });

            modelBuilder.Entity("recipes.MeasuredIngredient", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("NUMBER(10)");

                    OraclePropertyBuilderExtensions.UseIdentityColumn(b.Property<int?>("Id"));

                    b.Property<int>("IngredientId")
                        .HasColumnType("NUMBER(10)");

                    b.Property<double>("Quantity")
                        .HasColumnType("BINARY_DOUBLE");

                    b.Property<int?>("RecipeId")
                        .HasColumnType("NUMBER(10)");

                    b.HasKey("Id");

                    b.HasIndex("IngredientId");

                    b.HasIndex("RecipeId");

                    b.ToTable("RecipeManager_MeasuredIngredients");
                });

            modelBuilder.Entity("recipes.Recipe", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("NUMBER(10)");

                    OraclePropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CookTimeMins")
                        .HasColumnType("NUMBER(10)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<int>("NumberOfServings")
                        .HasColumnType("NUMBER(10)");

                    b.Property<int>("OwnerId")
                        .HasColumnType("NUMBER(10)");

                    b.Property<int>("PrepTimeMins")
                        .HasColumnType("NUMBER(10)");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.ToTable("RecipeManager_Recipes");
                });

            modelBuilder.Entity("users.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("NUMBER(10)");

                    OraclePropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"));

                    b.Property<string>("Description")
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<byte[]>("Image")
                        .HasColumnType("RAW(2000)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.HasKey("UserId");

                    b.ToTable("RecipeManager_Users");
                });

            modelBuilder.Entity("RecipeUser", b =>
                {
                    b.HasOne("recipes.Recipe", null)
                        .WithMany()
                        .HasForeignKey("UserFavoriteRecipiesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("users.User", null)
                        .WithMany()
                        .HasForeignKey("UserFavouriteUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Tag", b =>
                {
                    b.HasOne("recipes.Recipe", null)
                        .WithMany("Tags")
                        .HasForeignKey("RecipeId");
                });

            modelBuilder.Entity("recipes.Instruction", b =>
                {
                    b.HasOne("recipes.Recipe", null)
                        .WithMany("Instructions")
                        .HasForeignKey("RecipeId");
                });

            modelBuilder.Entity("recipes.MeasuredIngredient", b =>
                {
                    b.HasOne("recipes.Ingredient", "Ingredient")
                        .WithMany()
                        .HasForeignKey("IngredientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("recipes.Recipe", null)
                        .WithMany("Ingredients")
                        .HasForeignKey("RecipeId");

                    b.Navigation("Ingredient");
                });

            modelBuilder.Entity("recipes.Recipe", b =>
                {
                    b.HasOne("users.User", "Owner")
                        .WithMany("UserCreatedRecipies")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("recipes.Recipe", b =>
                {
                    b.Navigation("Ingredients");

                    b.Navigation("Instructions");

                    b.Navigation("Tags");
                });

            modelBuilder.Entity("users.User", b =>
                {
                    b.Navigation("UserCreatedRecipies");
                });
#pragma warning restore 612, 618
        }
    }
}
