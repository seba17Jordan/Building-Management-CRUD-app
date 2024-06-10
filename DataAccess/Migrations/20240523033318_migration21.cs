using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class migration21 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "managerId",
                table: "Buildings",
                newName: "ManagerId");

            migrationBuilder.AlterColumn<Guid>(
                name: "ManagerId",
                table: "Buildings",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateIndex(
                name: "IX_Buildings_ManagerId",
                table: "Buildings",
                column: "ManagerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Buildings_Users_ManagerId",
                table: "Buildings",
                column: "ManagerId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Buildings_Users_ManagerId",
                table: "Buildings");

            migrationBuilder.DropIndex(
                name: "IX_Buildings_ManagerId",
                table: "Buildings");

            migrationBuilder.RenameColumn(
                name: "ManagerId",
                table: "Buildings",
                newName: "managerId");

            migrationBuilder.AlterColumn<Guid>(
                name: "managerId",
                table: "Buildings",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);
        }
    }
}
