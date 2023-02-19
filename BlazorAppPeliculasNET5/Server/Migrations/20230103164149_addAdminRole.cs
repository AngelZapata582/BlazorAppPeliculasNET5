using Microsoft.EntityFrameworkCore.Migrations;

namespace BlazorAppPeliculasNET5.Server.Migrations
{
    public partial class addAdminRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "09da592c-d274-47f8-8e84-58301e1f0013", "64b85cf8-fef1-4881-af93-945d9283ea64", "admin", "admin" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "09da592c-d274-47f8-8e84-58301e1f0013");
        }
    }
}
