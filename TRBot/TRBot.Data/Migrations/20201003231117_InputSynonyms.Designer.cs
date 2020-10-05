﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TRBot.Data;

namespace TRBot.Data.Migrations
{
    [DbContext(typeof(BotDBContext))]
    [Migration("20201003231117_InputSynonyms")]
    partial class InputSynonyms
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.8");

            modelBuilder.Entity("TRBot.Data.CommandData", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("class_name")
                        .HasColumnType("TEXT");

                    b.Property<int>("display_in_list")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(1);

                    b.Property<int>("enabled")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(1);

                    b.Property<int>("level")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(0);

                    b.Property<string>("name")
                        .HasColumnType("TEXT");

                    b.Property<string>("value_str")
                        .HasColumnType("TEXT");

                    b.HasKey("id");

                    b.HasIndex("name")
                        .IsUnique();

                    b.ToTable("CommandData","commanddata");
                });

            modelBuilder.Entity("TRBot.Data.GameLog", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("LogDateTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("LogMessage")
                        .HasColumnType("TEXT");

                    b.Property<string>("User")
                        .HasColumnType("TEXT");

                    b.HasKey("id");

                    b.ToTable("GameLogs","gamelogs");
                });

            modelBuilder.Entity("TRBot.Data.Meme", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("MemeName")
                        .HasColumnType("TEXT");

                    b.Property<string>("MemeValue")
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.HasIndex("MemeName")
                        .IsUnique();

                    b.ToTable("Memes","memes");
                });

            modelBuilder.Entity("TRBot.Data.SavestateLog", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("LogDateTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("LogMessage")
                        .HasColumnType("TEXT");

                    b.Property<int>("SavestateNum")
                        .HasColumnType("INTEGER");

                    b.Property<string>("User")
                        .HasColumnType("TEXT");

                    b.HasKey("id");

                    b.ToTable("SavestateLogs","savestatelogs");
                });

            modelBuilder.Entity("TRBot.Data.Settings", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("key")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT")
                        .HasDefaultValue("");

                    b.Property<long>("value_int")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(0L);

                    b.Property<string>("value_str")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT")
                        .HasDefaultValue("");

                    b.HasKey("id");

                    b.HasIndex("key")
                        .IsUnique();

                    b.ToTable("Settings","settings");
                });

            modelBuilder.Entity("TRBot.ParserData.InputMacro", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("MacroName")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT")
                        .HasDefaultValue("");

                    b.Property<string>("MacroValue")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT")
                        .HasDefaultValue("");

                    b.HasKey("id");

                    b.HasIndex("MacroName")
                        .IsUnique();

                    b.ToTable("Macros","macros");
                });

            modelBuilder.Entity("TRBot.ParserData.InputSynonym", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("SynonymName")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT")
                        .HasDefaultValue("");

                    b.Property<string>("SynonymValue")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT")
                        .HasDefaultValue("");

                    b.HasKey("id");

                    b.HasIndex("SynonymName")
                        .IsUnique();

                    b.ToTable("InputSynonyms","inputsynonyms");
                });
#pragma warning restore 612, 618
        }
    }
}