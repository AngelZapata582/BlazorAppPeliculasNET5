using Microsoft.EntityFrameworkCore.Migrations;

namespace BlazorAppPeliculasNET5.Server.Migrations
{
    public partial class AddRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($"INSERT INTO AspNetRoles (Id,Name,NormalizedName)" +
                $"VALUES ('2dfd5e7f-e0bc-4086-a76b-4e1da41c8d83','admin','ADMIN')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE AspNetRoles WHERE Id = '2dfd5e7f-e0bc-4086-a76b-4e1da41c8d83'");
        }
    }
}
