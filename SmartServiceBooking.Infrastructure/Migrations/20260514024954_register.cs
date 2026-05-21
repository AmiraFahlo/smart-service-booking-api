using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartServiceBooking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class register : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Altering rowversion column is not supported by SQL Server.
            // This change is ignored as rowversion is managed by the DB.
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Altering rowversion column is not supported by SQL Server.
        }
    }
}
