using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartServiceBooking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddProviderAvailabilityUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProviderAvailability_ServiceProviders_ProviderId",
                schema: "Service",
                table: "ProviderAvailability");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProviderAvailability",
                schema: "Service",
                table: "ProviderAvailability");

            migrationBuilder.DropIndex(
                name: "IX_ProviderAvailability_ProviderId",
                schema: "Service",
                table: "ProviderAvailability");

            migrationBuilder.RenameTable(
                name: "ProviderAvailability",
                schema: "Service",
                newName: "ProviderAvailabilities",
                newSchema: "Service");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProviderAvailabilities",
                schema: "Service",
                table: "ProviderAvailabilities",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Services_BasePrice",
                schema: "Service",
                table: "Services",
                column: "BasePrice");

            migrationBuilder.CreateIndex(
                name: "IX_Services_Name",
                schema: "Service",
                table: "Services",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Name",
                schema: "Service",
                table: "Categories",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_ProviderAvailabilities_ProviderId_DayOfWeek",
                schema: "Service",
                table: "ProviderAvailabilities",
                columns: new[] { "ProviderId", "DayOfWeek" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProviderAvailabilities_ServiceProviders_ProviderId",
                schema: "Service",
                table: "ProviderAvailabilities",
                column: "ProviderId",
                principalSchema: "Service",
                principalTable: "ServiceProviders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProviderAvailabilities_ServiceProviders_ProviderId",
                schema: "Service",
                table: "ProviderAvailabilities");

            migrationBuilder.DropIndex(
                name: "IX_Services_BasePrice",
                schema: "Service",
                table: "Services");

            migrationBuilder.DropIndex(
                name: "IX_Services_Name",
                schema: "Service",
                table: "Services");

            migrationBuilder.DropIndex(
                name: "IX_Categories_Name",
                schema: "Service",
                table: "Categories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProviderAvailabilities",
                schema: "Service",
                table: "ProviderAvailabilities");

            migrationBuilder.DropIndex(
                name: "IX_ProviderAvailabilities_ProviderId_DayOfWeek",
                schema: "Service",
                table: "ProviderAvailabilities");

            migrationBuilder.RenameTable(
                name: "ProviderAvailabilities",
                schema: "Service",
                newName: "ProviderAvailability",
                newSchema: "Service");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProviderAvailability",
                schema: "Service",
                table: "ProviderAvailability",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ProviderAvailability_ProviderId",
                schema: "Service",
                table: "ProviderAvailability",
                column: "ProviderId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProviderAvailability_ServiceProviders_ProviderId",
                schema: "Service",
                table: "ProviderAvailability",
                column: "ProviderId",
                principalSchema: "Service",
                principalTable: "ServiceProviders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
