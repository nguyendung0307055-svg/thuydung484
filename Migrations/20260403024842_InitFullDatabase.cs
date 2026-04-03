using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace thuydung484.Migrations
{
    /// <inheritdoc />
    public partial class InitFullDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "branch_id",
                table: "Vehicles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Branchid",
                table: "Rentals",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Userid",
                table: "Rentals",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Branch",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    phone = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    manager_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branch", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Payment_Logs",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    payment_id = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    note = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payment_Logs", x => x.id);
                    table.ForeignKey(
                        name: "FK_Payment_Logs_Payments_payment_id",
                        column: x => x.payment_id,
                        principalTable: "Payments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Penalty",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    rental_id = table.Column<int>(type: "int", nullable: false),
                    penalty_type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    amount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Penalty", x => x.id);
                    table.ForeignKey(
                        name: "FK_Penalty_Rentals_rental_id",
                        column: x => x.rental_id,
                        principalTable: "Rentals",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Rental_Detail",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    rental_id = table.Column<int>(type: "int", nullable: false),
                    price_per_hour = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    price_per_day = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    estimated_amount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    actual_amount = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    penalty_amount = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rental_Detail", x => x.id);
                    table.ForeignKey(
                        name: "FK_Rental_Detail_Rentals_rental_id",
                        column: x => x.rental_id,
                        principalTable: "Rentals",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Vehicle_Images",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    vehicle_id = table.Column<int>(type: "int", nullable: false),
                    image_url = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    is_primary = table.Column<bool>(type: "bit", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicle_Images", x => x.id);
                    table.ForeignKey(
                        name: "FK_Vehicle_Images_Vehicles_vehicle_id",
                        column: x => x.vehicle_id,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    password_hash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    phone = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    role = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    branch_id = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.id);
                    table.ForeignKey(
                        name: "FK_User_Branch_branch_id",
                        column: x => x.branch_id,
                        principalTable: "Branch",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Vehicle_Maintenance",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    vehicle_id = table.Column<int>(type: "int", nullable: false),
                    description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    start_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    end_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    cost = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicle_Maintenance", x => x.id);
                    table.ForeignKey(
                        name: "FK_Vehicle_Maintenance_User_user_id",
                        column: x => x.user_id,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Vehicle_Maintenance_Vehicles_vehicle_id",
                        column: x => x.vehicle_id,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Vehicle_Status_Logs",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    vehicle_id = table.Column<int>(type: "int", nullable: false),
                    old_status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    new_status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    changed_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    changed_by = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicle_Status_Logs", x => x.id);
                    table.ForeignKey(
                        name: "FK_Vehicle_Status_Logs_User_changed_by",
                        column: x => x.changed_by,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Vehicle_Status_Logs_Vehicles_vehicle_id",
                        column: x => x.vehicle_id,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_branch_id",
                table: "Vehicles",
                column: "branch_id");

            migrationBuilder.CreateIndex(
                name: "IX_Rentals_Branchid",
                table: "Rentals",
                column: "Branchid");

            migrationBuilder.CreateIndex(
                name: "IX_Rentals_Userid",
                table: "Rentals",
                column: "Userid");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_Logs_payment_id",
                table: "Payment_Logs",
                column: "payment_id");

            migrationBuilder.CreateIndex(
                name: "IX_Penalty_rental_id",
                table: "Penalty",
                column: "rental_id");

            migrationBuilder.CreateIndex(
                name: "IX_Rental_Detail_rental_id",
                table: "Rental_Detail",
                column: "rental_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_branch_id",
                table: "User",
                column: "branch_id");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicle_Images_vehicle_id",
                table: "Vehicle_Images",
                column: "vehicle_id");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicle_Maintenance_user_id",
                table: "Vehicle_Maintenance",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicle_Maintenance_vehicle_id",
                table: "Vehicle_Maintenance",
                column: "vehicle_id");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicle_Status_Logs_changed_by",
                table: "Vehicle_Status_Logs",
                column: "changed_by");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicle_Status_Logs_vehicle_id",
                table: "Vehicle_Status_Logs",
                column: "vehicle_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Rentals_Branch_Branchid",
                table: "Rentals",
                column: "Branchid",
                principalTable: "Branch",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Rentals_User_Userid",
                table: "Rentals",
                column: "Userid",
                principalTable: "User",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_Branch_branch_id",
                table: "Vehicles",
                column: "branch_id",
                principalTable: "Branch",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rentals_Branch_Branchid",
                table: "Rentals");

            migrationBuilder.DropForeignKey(
                name: "FK_Rentals_User_Userid",
                table: "Rentals");

            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_Branch_branch_id",
                table: "Vehicles");

            migrationBuilder.DropTable(
                name: "Payment_Logs");

            migrationBuilder.DropTable(
                name: "Penalty");

            migrationBuilder.DropTable(
                name: "Rental_Detail");

            migrationBuilder.DropTable(
                name: "Vehicle_Images");

            migrationBuilder.DropTable(
                name: "Vehicle_Maintenance");

            migrationBuilder.DropTable(
                name: "Vehicle_Status_Logs");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Branch");

            migrationBuilder.DropIndex(
                name: "IX_Vehicles_branch_id",
                table: "Vehicles");

            migrationBuilder.DropIndex(
                name: "IX_Rentals_Branchid",
                table: "Rentals");

            migrationBuilder.DropIndex(
                name: "IX_Rentals_Userid",
                table: "Rentals");

            migrationBuilder.DropColumn(
                name: "branch_id",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "Branchid",
                table: "Rentals");

            migrationBuilder.DropColumn(
                name: "Userid",
                table: "Rentals");
        }
    }
}
