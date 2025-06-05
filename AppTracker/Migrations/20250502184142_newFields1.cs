using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LauncherApp.Migrations
{
    /// <inheritdoc />
    public partial class newFields1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "newtestField",
                table: "AppMs",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "newtestField",
                table: "AppMs");
        }
    }
}
