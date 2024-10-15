﻿// <auto-generated />
using System;
using Flashcard.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Flashcard.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20241015131049_AdicionandoTabela")]
    partial class AdicionandoTabela
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Flashcard.Models.BaralhoModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("ContadorFlashcards")
                        .HasColumnType("integer");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("Id");

                    b.ToTable("Baralhos");
                });

            modelBuilder.Entity("Flashcard.Models.CategoriaModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Categorias");
                });

            modelBuilder.Entity("Flashcard.Models.FlashcardModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("BaralhoId")
                        .HasColumnType("integer");

                    b.Property<int>("CategoriaId")
                        .HasColumnType("integer");

                    b.Property<string>("Pergunta")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Resposta")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("RevisaoId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("ultimaRevisao")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("BaralhoId");

                    b.HasIndex("CategoriaId");

                    b.HasIndex("RevisaoId")
                        .IsUnique();

                    b.ToTable("Flashcards");
                });

            modelBuilder.Entity("Flashcard.Models.RevisaoModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DataCriacao")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("FlashcardId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("proximaRevisao")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("revisoesRealizadas")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Revisoes");
                });

            modelBuilder.Entity("Flashcard.Models.FlashcardModel", b =>
                {
                    b.HasOne("Flashcard.Models.BaralhoModel", "Baralho")
                        .WithMany("Flashcards")
                        .HasForeignKey("BaralhoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Flashcard.Models.CategoriaModel", "Categoria")
                        .WithMany("Flashcards")
                        .HasForeignKey("CategoriaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Flashcard.Models.RevisaoModel", "Revisao")
                        .WithOne("Flashcard")
                        .HasForeignKey("Flashcard.Models.FlashcardModel", "RevisaoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Baralho");

                    b.Navigation("Categoria");

                    b.Navigation("Revisao");
                });

            modelBuilder.Entity("Flashcard.Models.BaralhoModel", b =>
                {
                    b.Navigation("Flashcards");
                });

            modelBuilder.Entity("Flashcard.Models.CategoriaModel", b =>
                {
                    b.Navigation("Flashcards");
                });

            modelBuilder.Entity("Flashcard.Models.RevisaoModel", b =>
                {
                    b.Navigation("Flashcard")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
