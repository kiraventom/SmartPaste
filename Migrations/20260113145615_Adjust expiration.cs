using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartPaste.Migrations
{
    /// <inheritdoc />
    public partial class Adjustexpiration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OneShot",
                table: "Pastes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "OneShot",
                table: "Pastes",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }
    }
}
