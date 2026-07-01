using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartServiceBooking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddReviewSystemUpdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Users_RateeId",
                schema: "Interaction",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Users_ReviewerId",
                schema: "Interaction",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_BookingId_ReviewerId",
                schema: "Interaction",
                table: "Reviews");

            migrationBuilder.AlterColumn<int>(
                name: "ReviewerId",
                schema: "Interaction",
                table: "Reviews",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_BookingId_ReviewerId",
                schema: "Interaction",
                table: "Reviews",
                columns: new[] { "BookingId", "ReviewerId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_ServiceProviders_RateeId",
                schema: "Interaction",
                table: "Reviews",
                column: "RateeId",
                principalSchema: "Service",
                principalTable: "ServiceProviders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Users_ReviewerId",
                schema: "Interaction",
                table: "Reviews",
                column: "ReviewerId",
                principalSchema: "Identity",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_ServiceProviders_RateeId",
                schema: "Interaction",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Users_ReviewerId",
                schema: "Interaction",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_BookingId_ReviewerId",
                schema: "Interaction",
                table: "Reviews");

            migrationBuilder.AlterColumn<int>(
                name: "ReviewerId",
                schema: "Interaction",
                table: "Reviews",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_BookingId_ReviewerId",
                schema: "Interaction",
                table: "Reviews",
                columns: new[] { "BookingId", "ReviewerId" },
                unique: true,
                filter: "[ReviewerId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Users_RateeId",
                schema: "Interaction",
                table: "Reviews",
                column: "RateeId",
                principalSchema: "Identity",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Users_ReviewerId",
                schema: "Interaction",
                table: "Reviews",
                column: "ReviewerId",
                principalSchema: "Identity",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
