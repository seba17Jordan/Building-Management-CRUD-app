using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class migration18 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConstructionCompany",
                table: "Buildings");

            migrationBuilder.AddColumn<Guid>(
                name: "ConstructionCompanyAdminId",
                table: "Buildings",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ConstructionCompanyId",
                table: "Buildings",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ConstructionCompanies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConstructionCompanyAdminId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConstructionCompanies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConstructionCompanies_Users_ConstructionCompanyAdminId",
                        column: x => x.ConstructionCompanyAdminId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Buildings_ConstructionCompanyAdminId",
                table: "Buildings",
                column: "ConstructionCompanyAdminId");

            migrationBuilder.CreateIndex(
                name: "IX_Buildings_ConstructionCompanyId",
                table: "Buildings",
                column: "ConstructionCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_ConstructionCompanies_ConstructionCompanyAdminId",
                table: "ConstructionCompanies",
                column: "ConstructionCompanyAdminId");

            migrationBuilder.AddForeignKey(
                name: "FK_Buildings_ConstructionCompanies_ConstructionCompanyId",
                table: "Buildings",
                column: "ConstructionCompanyId",
                principalTable: "ConstructionCompanies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Buildings_Users_ConstructionCompanyAdminId",
                table: "Buildings",
                column: "ConstructionCompanyAdminId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Buildings_ConstructionCompanies_ConstructionCompanyId",
                table: "Buildings");

            migrationBuilder.DropForeignKey(
                name: "FK_Buildings_Users_ConstructionCompanyAdminId",
                table: "Buildings");

            migrationBuilder.DropTable(
                name: "ConstructionCompanies");

            migrationBuilder.DropIndex(
                name: "IX_Buildings_ConstructionCompanyAdminId",
                table: "Buildings");

            migrationBuilder.DropIndex(
                name: "IX_Buildings_ConstructionCompanyId",
                table: "Buildings");

            migrationBuilder.DropColumn(
                name: "ConstructionCompanyAdminId",
                table: "Buildings");

            migrationBuilder.DropColumn(
                name: "ConstructionCompanyId",
                table: "Buildings");

            migrationBuilder.AddColumn<string>(
                name: "ConstructionCompany",
                table: "Buildings",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
