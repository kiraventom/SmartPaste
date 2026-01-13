using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartPaste.Migrations
{
    /// <inheritdoc />
    public partial class Addexpiration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ExpiresHours",
                table: "Pastes",
                newName: "OneShot");

            migrationBuilder.AddColumn<int>(
                name: "ExpiresMin",
                table: "Pastes",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpiresMin",
                table: "Pastes");

            migrationBuilder.RenameColumn(
                name: "OneShot",
                table: "Pastes",
                newName: "ExpiresHours");
        }
    }
}
