using Microsoft.EntityFrameworkCore.Migrations;

namespace AquaparkApplication.Migrations
{
    public partial class SeedTickets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Tickets",
                columns: new[] { "Id", "Days", "EndHour", "Months", "Name", "PeriodicDiscountId", "Price", "StartHour", "ZoneId" },
                values: new object[,]
                {
                    { 1, 1, 12.0, 0, "Basen - Bilet poranny 6:00-12:00", null, 30.00m, 6.0, 2 },
                    { 2, 1, 18.0, 0, "Basen - Bilet poranny 12:00-18:00", null, 35.00m, 12.0, 2 },
                    { 3, 1, 24.0, 0, "Basen - Bilet poranny 18:00-24:00", null, 40.00m, 18.0, 2 },
                    { 4, 1, 24.0, 0, "Basen - Bilet całodniowy", null, 60.00m, 0.0, 2 },
                    { 5, 1, 24.0, 0, "Sauna - Bilet całodniowy", null, 80.00m, 0.0, 1 },
                    { 6, 1, 24.0, 0, "Spa - Bilet całodniowy", null, 200.00m, 0.0, 3 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 6);
        }
    }
}
