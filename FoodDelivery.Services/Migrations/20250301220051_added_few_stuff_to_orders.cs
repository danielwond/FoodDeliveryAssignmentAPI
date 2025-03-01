using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodDelivery.Services.Migrations
{
    /// <inheritdoc />
    public partial class added_few_stuff_to_orders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryTrackings_Orders_OrderId",
                table: "DeliveryTrackings");

            migrationBuilder.DropIndex(
                name: "IX_DeliveryTrackings_OrderId",
                table: "DeliveryTrackings");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "DeliveryTrackings");

            migrationBuilder.AddColumn<string>(
                name: "DeliveryLocationLatitude",
                table: "Orders",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "DeliveryLocationLongitude",
                table: "Orders",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<Guid>(
                name: "DeliveryTrackingId",
                table: "Orders",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AlterColumn<string>(
                name: "Longitude",
                table: "DeliveryTrackings",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Latitude",
                table: "DeliveryTrackings",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "DeliveryTrackings",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.CreateTable(
                name: "Configurations",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ConfigurationsEnum = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configurations", x => x.ID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_DeliveryTrackingId",
                table: "Orders",
                column: "DeliveryTrackingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_DeliveryTrackings_DeliveryTrackingId",
                table: "Orders",
                column: "DeliveryTrackingId",
                principalTable: "DeliveryTrackings",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_DeliveryTrackings_DeliveryTrackingId",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "Configurations");

            migrationBuilder.DropIndex(
                name: "IX_Orders_DeliveryTrackingId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DeliveryLocationLatitude",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DeliveryLocationLongitude",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DeliveryTrackingId",
                table: "Orders");

            migrationBuilder.AlterColumn<double>(
                name: "Longitude",
                table: "DeliveryTrackings",
                type: "double",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<double>(
                name: "Latitude",
                table: "DeliveryTrackings",
                type: "double",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "DeliveryTrackings",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AddColumn<Guid>(
                name: "OrderId",
                table: "DeliveryTrackings",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryTrackings_OrderId",
                table: "DeliveryTrackings",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryTrackings_Orders_OrderId",
                table: "DeliveryTrackings",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
