using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BikeRentalAPI.Migrations
{
    /// <inheritdoc />
    public partial class Second : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RentalSchedule_Bike_BikeId",
                table: "RentalSchedule");

            migrationBuilder.DropForeignKey(
                name: "FK_RentalSchedule_UserInfo_UserInfoId",
                table: "RentalSchedule");

            migrationBuilder.DropIndex(
                name: "IX_RentalSchedule_BikeId",
                table: "RentalSchedule");

            migrationBuilder.DropIndex(
                name: "IX_RentalSchedule_UserInfoId",
                table: "RentalSchedule");

            migrationBuilder.DropColumn(
                name: "UserInfoId",
                table: "RentalSchedule");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserInfoId",
                table: "RentalSchedule",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_RentalSchedule_BikeId",
                table: "RentalSchedule",
                column: "BikeId");

            migrationBuilder.CreateIndex(
                name: "IX_RentalSchedule_UserInfoId",
                table: "RentalSchedule",
                column: "UserInfoId");

            migrationBuilder.AddForeignKey(
                name: "FK_RentalSchedule_Bike_BikeId",
                table: "RentalSchedule",
                column: "BikeId",
                principalTable: "Bike",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RentalSchedule_UserInfo_UserInfoId",
                table: "RentalSchedule",
                column: "UserInfoId",
                principalTable: "UserInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
