using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CouponSystem.Migrations
{
    /// <inheritdoc />
    public partial class CouponSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Coupons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AmountOff = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    PercentOff = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CombinedDiscounts = table.Column<bool>(type: "bit", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsesCount = table.Column<int>(type: "int", nullable: false),
                    MaxUses = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coupons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Coupons_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "bf3b9da8-7434-4409-aac3-502d18baf972",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ee805e21-9bb9-4796-b244-24f495ae8b70", "AQAAAAIAAYagAAAAEMh/P+zCbcqf1Xhr+jBVrRynh3cUAbZOczCEGq7HyYRyHKz1mYlE9i7fXikuUzG76Q==", "72dccc3d-7cab-4ce6-af59-b198bb087aa6" });

            migrationBuilder.CreateIndex(
                name: "IX_Coupons_UserId",
                table: "Coupons",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Coupons");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "bf3b9da8-7434-4409-aac3-502d18baf972",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "34d6460f-3965-41c6-ac0c-e4cfdc367841", "AQAAAAIAAYagAAAAENnjYctaBlUcTRmEN5BmFFYC0bDpmuZFEMYbiHm6xtL3d1XKnYiSNNSQoIn5aSZgOQ==", "a9410818-173d-44ae-bfd3-c60857d3c1ba" });
        }
    }
}
