using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AquaparkApplication.Migrations
{
    public partial class SeedPeriodicDiscount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "PeriodicDiscounts",
                columns: new[] { "Id", "FinishTime", "StartTime", "Value" },
                values: new object[] { 1, new DateTime(2019, 5, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2019, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0.80m });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PeriodicDiscounts",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
