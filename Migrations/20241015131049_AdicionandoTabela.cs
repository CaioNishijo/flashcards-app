using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Flashcard.Migrations
{
    /// <inheritdoc />
    public partial class AdicionandoTabela : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flashcards_RevisaoModel_RevisaoId",
                table: "Flashcards");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RevisaoModel",
                table: "RevisaoModel");

            migrationBuilder.RenameTable(
                name: "RevisaoModel",
                newName: "Revisoes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Revisoes",
                table: "Revisoes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Flashcards_Revisoes_RevisaoId",
                table: "Flashcards",
                column: "RevisaoId",
                principalTable: "Revisoes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flashcards_Revisoes_RevisaoId",
                table: "Flashcards");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Revisoes",
                table: "Revisoes");

            migrationBuilder.RenameTable(
                name: "Revisoes",
                newName: "RevisaoModel");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RevisaoModel",
                table: "RevisaoModel",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Flashcards_RevisaoModel_RevisaoId",
                table: "Flashcards",
                column: "RevisaoId",
                principalTable: "RevisaoModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
