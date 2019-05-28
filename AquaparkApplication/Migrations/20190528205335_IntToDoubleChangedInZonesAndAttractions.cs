using Microsoft.EntityFrameworkCore.Migrations;

namespace AquaparkApplication.Migrations
{
    public partial class IntToDoubleChangedInZonesAndAttractions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "MaxAmountOfPeople",
                table: "Zones",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<double>(
                name: "MaxAmountOfPeople",
                table: "Attractions",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.UpdateData(
                table: "Attractions",
                keyColumn: "Id",
                keyValue: 1,
                column: "MaxAmountOfPeople",
                value: 5.0);

            migrationBuilder.UpdateData(
                table: "Attractions",
                keyColumn: "Id",
                keyValue: 2,
                column: "MaxAmountOfPeople",
                value: 5.0);

            migrationBuilder.UpdateData(
                table: "Attractions",
                keyColumn: "Id",
                keyValue: 3,
                column: "MaxAmountOfPeople",
                value: 5.0);

            migrationBuilder.UpdateData(
                table: "Attractions",
                keyColumn: "Id",
                keyValue: 4,
                column: "MaxAmountOfPeople",
                value: 7.0);

            migrationBuilder.UpdateData(
                table: "Attractions",
                keyColumn: "Id",
                keyValue: 5,
                column: "MaxAmountOfPeople",
                value: 3.0);

            migrationBuilder.UpdateData(
                table: "Attractions",
                keyColumn: "Id",
                keyValue: 6,
                column: "MaxAmountOfPeople",
                value: 2.0);

            migrationBuilder.UpdateData(
                table: "Attractions",
                keyColumn: "Id",
                keyValue: 7,
                column: "MaxAmountOfPeople",
                value: 8.0);

            migrationBuilder.UpdateData(
                table: "Attractions",
                keyColumn: "Id",
                keyValue: 8,
                column: "MaxAmountOfPeople",
                value: 25.0);

            migrationBuilder.UpdateData(
                table: "Attractions",
                keyColumn: "Id",
                keyValue: 9,
                column: "MaxAmountOfPeople",
                value: 25.0);

            migrationBuilder.UpdateData(
                table: "Attractions",
                keyColumn: "Id",
                keyValue: 10,
                column: "MaxAmountOfPeople",
                value: 30.0);

            migrationBuilder.UpdateData(
                table: "Attractions",
                keyColumn: "Id",
                keyValue: 11,
                column: "MaxAmountOfPeople",
                value: 20.0);

            migrationBuilder.UpdateData(
                table: "Attractions",
                keyColumn: "Id",
                keyValue: 12,
                column: "MaxAmountOfPeople",
                value: 5.0);

            migrationBuilder.UpdateData(
                table: "Attractions",
                keyColumn: "Id",
                keyValue: 13,
                column: "MaxAmountOfPeople",
                value: 5.0);

            migrationBuilder.UpdateData(
                table: "Attractions",
                keyColumn: "Id",
                keyValue: 14,
                column: "MaxAmountOfPeople",
                value: 10.0);

            migrationBuilder.UpdateData(
                table: "Zones",
                keyColumn: "Id",
                keyValue: 1,
                column: "MaxAmountOfPeople",
                value: 35.0);

            migrationBuilder.UpdateData(
                table: "Zones",
                keyColumn: "Id",
                keyValue: 2,
                column: "MaxAmountOfPeople",
                value: 100.0);

            migrationBuilder.UpdateData(
                table: "Zones",
                keyColumn: "Id",
                keyValue: 3,
                column: "MaxAmountOfPeople",
                value: 20.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "MaxAmountOfPeople",
                table: "Zones",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<int>(
                name: "MaxAmountOfPeople",
                table: "Attractions",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.UpdateData(
                table: "Attractions",
                keyColumn: "Id",
                keyValue: 1,
                column: "MaxAmountOfPeople",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Attractions",
                keyColumn: "Id",
                keyValue: 2,
                column: "MaxAmountOfPeople",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Attractions",
                keyColumn: "Id",
                keyValue: 3,
                column: "MaxAmountOfPeople",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Attractions",
                keyColumn: "Id",
                keyValue: 4,
                column: "MaxAmountOfPeople",
                value: 7);

            migrationBuilder.UpdateData(
                table: "Attractions",
                keyColumn: "Id",
                keyValue: 5,
                column: "MaxAmountOfPeople",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Attractions",
                keyColumn: "Id",
                keyValue: 6,
                column: "MaxAmountOfPeople",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Attractions",
                keyColumn: "Id",
                keyValue: 7,
                column: "MaxAmountOfPeople",
                value: 8);

            migrationBuilder.UpdateData(
                table: "Attractions",
                keyColumn: "Id",
                keyValue: 8,
                column: "MaxAmountOfPeople",
                value: 25);

            migrationBuilder.UpdateData(
                table: "Attractions",
                keyColumn: "Id",
                keyValue: 9,
                column: "MaxAmountOfPeople",
                value: 25);

            migrationBuilder.UpdateData(
                table: "Attractions",
                keyColumn: "Id",
                keyValue: 10,
                column: "MaxAmountOfPeople",
                value: 30);

            migrationBuilder.UpdateData(
                table: "Attractions",
                keyColumn: "Id",
                keyValue: 11,
                column: "MaxAmountOfPeople",
                value: 20);

            migrationBuilder.UpdateData(
                table: "Attractions",
                keyColumn: "Id",
                keyValue: 12,
                column: "MaxAmountOfPeople",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Attractions",
                keyColumn: "Id",
                keyValue: 13,
                column: "MaxAmountOfPeople",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Attractions",
                keyColumn: "Id",
                keyValue: 14,
                column: "MaxAmountOfPeople",
                value: 10);

            migrationBuilder.UpdateData(
                table: "Zones",
                keyColumn: "Id",
                keyValue: 1,
                column: "MaxAmountOfPeople",
                value: 35);

            migrationBuilder.UpdateData(
                table: "Zones",
                keyColumn: "Id",
                keyValue: 2,
                column: "MaxAmountOfPeople",
                value: 100);

            migrationBuilder.UpdateData(
                table: "Zones",
                keyColumn: "Id",
                keyValue: 3,
                column: "MaxAmountOfPeople",
                value: 20);
        }
    }
}
