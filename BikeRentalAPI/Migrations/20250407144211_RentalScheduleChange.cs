using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BikeRentalAPI.Migrations
{
    /// <inheritdoc />
    public partial class RentalScheduleChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isCancelled",
                table: "RentalSchedule",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isCancelled",
                table: "RentalSchedule");
        }
    }
}
