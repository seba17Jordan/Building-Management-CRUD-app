using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class migration13 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BuildingId",
                table: "ServiceRequests",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "managerId",
                table: "Buildings",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Buildings_managerId",
                table: "Buildings",
                column: "managerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Buildings_Users_managerId",
                table: "Buildings",
                column: "managerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Buildings_Users_managerId",
                table: "Buildings");

            migrationBuilder.DropIndex(
                name: "IX_Buildings_managerId",
                table: "Buildings");

            migrationBuilder.DropColumn(
                name: "BuildingId",
                table: "ServiceRequests");

            migrationBuilder.DropColumn(
                name: "managerId",
                table: "Buildings");
        }
    }
}
