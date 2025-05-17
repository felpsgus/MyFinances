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
            migrationBuilder.AddColumn<Guid>(
                name: "TagId1",
                table: "ExpenseTag",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseTag_TagId1",
                table: "ExpenseTag",
                column: "TagId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ExpenseTag_Tag_TagId1",
                table: "ExpenseTag",
                column: "TagId1",
                principalTable: "Tag",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExpenseTag_Tag_TagId1",
                table: "ExpenseTag");

            migrationBuilder.DropIndex(
                name: "IX_ExpenseTag_TagId1",
                table: "ExpenseTag");

            migrationBuilder.DropColumn(
                name: "TagId1",
                table: "ExpenseTag");
        }
    }
}
