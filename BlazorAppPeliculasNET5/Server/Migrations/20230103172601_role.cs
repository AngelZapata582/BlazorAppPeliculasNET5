using Microsoft.EntityFrameworkCore.Migrations;

namespace BlazorAppPeliculasNET5.Server.Migrations
{
    public partial class role : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "09da592c-d274-47f8-8e84-58301e1f0013",
                column: "ConcurrencyStamp",
                value: "5a539317-794e-413c-985b-764ecede30e6");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "09da592c-d274-47f8-8e84-58301e1f0013",
                column: "ConcurrencyStamp",
                value: "64b85cf8-fef1-4881-af93-945d9283ea64");
        }
    }
}
