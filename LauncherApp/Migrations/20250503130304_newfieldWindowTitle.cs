using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LauncherApp.Migrations
{
    /// <inheritdoc />
    public partial class newfieldWindowTitle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "WindowTitle",
                table: "AppMs",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WindowTitle",
                table: "AppMs");
        }
    }
}
