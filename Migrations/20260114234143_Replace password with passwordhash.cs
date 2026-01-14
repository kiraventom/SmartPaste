using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartPaste.Migrations
{
    /// <inheritdoc />
    public partial class Replacepasswordwithpasswordhash : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Pastes",
                newName: "PasswordHash");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                table: "Pastes",
                newName: "Password");
        }
    }
}
