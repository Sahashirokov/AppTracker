using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LauncherApp.Migrations
{
    /// <inheritdoc />
    public partial class deleteTestField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "newtestField",
                table: "AppMs");

            migrationBuilder.DropColumn(
                name: "testField",
                table: "AppMs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "newtestField",
                table: "AppMs",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "testField",
                table: "AppMs",
                type: "TEXT",
                nullable: true);
        }
    }
}
