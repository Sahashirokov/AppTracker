using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LauncherApp.Migrations
{
    /// <inheritdoc />
    public partial class forWeek : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeSpan>(
                name: "forWeek",
                table: "AppMs",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "forWeek",
                table: "AppMs");
        }
    }
}
