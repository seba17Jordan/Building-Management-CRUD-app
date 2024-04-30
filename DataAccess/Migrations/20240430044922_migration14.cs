using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class migration14 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Buildings_Users_managerId",
                table: "Buildings");

            migrationBuilder.DropIndex(
                name: "IX_Buildings_managerId",
                table: "Buildings");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
