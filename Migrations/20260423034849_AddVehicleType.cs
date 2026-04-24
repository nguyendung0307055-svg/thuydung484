using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace thuydung484.Migrations
{
    /// <inheritdoc />
    public partial class AddVehicleType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "type",
                table: "Vehicles");

            migrationBuilder.AddColumn<int>(
                name: "vehicle_type_id",
                table: "Vehicles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "VehicleTypes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleTypes", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_vehicle_type_id",
                table: "Vehicles",
                column: "vehicle_type_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_VehicleTypes_vehicle_type_id",
                table: "Vehicles",
                column: "vehicle_type_id",
                principalTable: "VehicleTypes",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_VehicleTypes_vehicle_type_id",
                table: "Vehicles");

            migrationBuilder.DropTable(
                name: "VehicleTypes");

            migrationBuilder.DropIndex(
                name: "IX_Vehicles_vehicle_type_id",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "vehicle_type_id",
                table: "Vehicles");

            migrationBuilder.AddColumn<string>(
                name: "type",
                table: "Vehicles",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }
    }
}
