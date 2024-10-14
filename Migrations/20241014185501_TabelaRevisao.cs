using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Flashcard.Migrations
{
    /// <inheritdoc />
    public partial class TabelaRevisao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DataCriacao",
                table: "Flashcards",
                newName: "ultimaRevisao");

            migrationBuilder.AddColumn<int>(
                name: "RevisaoId",
                table: "Flashcards",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "RevisaoModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    proximaRevisao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    revisoesRealizadas = table.Column<int>(type: "integer", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FlashcardId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RevisaoModel", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Flashcards_RevisaoId",
                table: "Flashcards",
                column: "RevisaoId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Flashcards_RevisaoModel_RevisaoId",
                table: "Flashcards",
                column: "RevisaoId",
                principalTable: "RevisaoModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flashcards_RevisaoModel_RevisaoId",
                table: "Flashcards");

            migrationBuilder.DropTable(
                name: "RevisaoModel");

            migrationBuilder.DropIndex(
                name: "IX_Flashcards_RevisaoId",
                table: "Flashcards");

            migrationBuilder.DropColumn(
                name: "RevisaoId",
                table: "Flashcards");

            migrationBuilder.RenameColumn(
                name: "ultimaRevisao",
                table: "Flashcards",
                newName: "DataCriacao");
        }
    }
}
