using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MagicVilla_API.Migrations
{
    public partial class AlimentarTablaVilla : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Villas",
                columns: new[] { "Id", "Amenidad", "Detalle", "FechaActualizacion", "FechaCreacion", "ImageUrl", "MetrosCuadrados", "Nombre", "Ocupantes", "Tarifa" },
                values: new object[] { 1, "", "Detalle de la Villa...", new DateTime(2024, 10, 15, 15, 46, 45, 822, DateTimeKind.Local).AddTicks(302), new DateTime(2024, 10, 15, 15, 46, 45, 822, DateTimeKind.Local).AddTicks(291), "", 50.0, "Villa Real", 5, 200.0 });

            migrationBuilder.InsertData(
                table: "Villas",
                columns: new[] { "Id", "Amenidad", "Detalle", "FechaActualizacion", "FechaCreacion", "ImageUrl", "MetrosCuadrados", "Nombre", "Ocupantes", "Tarifa" },
                values: new object[] { 2, "", "Detalle de la Villa...", new DateTime(2024, 10, 15, 15, 46, 45, 822, DateTimeKind.Local).AddTicks(308), new DateTime(2024, 10, 15, 15, 46, 45, 822, DateTimeKind.Local).AddTicks(307), "", 40.0, "Premium Vista a la piscina", 4, 150.0 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
