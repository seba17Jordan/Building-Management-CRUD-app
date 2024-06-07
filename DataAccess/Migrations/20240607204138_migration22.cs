using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class migration22 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CategoryName",
                table: "ServiceRequests");

            migrationBuilder.RenameColumn(
                name: "Category",
                table: "ServiceRequests",
                newName: "CategoryId");

            migrationBuilder.RenameColumn(
                name: "Apartment",
                table: "ServiceRequests",
                newName: "ApartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRequests_ApartmentId",
                table: "ServiceRequests",
                column: "ApartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRequests_BuildingId",
                table: "ServiceRequests",
                column: "BuildingId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRequests_CategoryId",
                table: "ServiceRequests",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceRequests_Apartments_ApartmentId",
                table: "ServiceRequests",
                column: "ApartmentId",
                principalTable: "Apartments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceRequests_Buildings_BuildingId",
                table: "ServiceRequests",
                column: "BuildingId",
                principalTable: "Buildings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceRequests_Categories_CategoryId",
                table: "ServiceRequests",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceRequests_Apartments_ApartmentId",
                table: "ServiceRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceRequests_Buildings_BuildingId",
                table: "ServiceRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceRequests_Categories_CategoryId",
                table: "ServiceRequests");

            migrationBuilder.DropIndex(
                name: "IX_ServiceRequests_ApartmentId",
                table: "ServiceRequests");

            migrationBuilder.DropIndex(
                name: "IX_ServiceRequests_BuildingId",
                table: "ServiceRequests");

            migrationBuilder.DropIndex(
                name: "IX_ServiceRequests_CategoryId",
                table: "ServiceRequests");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "ServiceRequests",
                newName: "Category");

            migrationBuilder.RenameColumn(
                name: "ApartmentId",
                table: "ServiceRequests",
                newName: "Apartment");

            migrationBuilder.AddColumn<string>(
                name: "CategoryName",
                table: "ServiceRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
