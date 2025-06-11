using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyFinances.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class _9 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tag_Expense_ExpenseId",
                table: "Tag");

            migrationBuilder.DropIndex(
                name: "IX_Tag_ExpenseId",
                table: "Tag");

            migrationBuilder.DropColumn(
                name: "ExpenseId",
                table: "Tag");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ExpenseId",
                table: "Tag",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tag_ExpenseId",
                table: "Tag",
                column: "ExpenseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tag_Expense_ExpenseId",
                table: "Tag",
                column: "ExpenseId",
                principalTable: "Expense",
                principalColumn: "Id");
        }
    }
}
