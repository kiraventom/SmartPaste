using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartPaste.Migrations
{
    /// <inheritdoc />
    public partial class Password : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Pastes",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                table: "Pastes");
        }
    }
}
