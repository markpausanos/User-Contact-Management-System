using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace User_Contact_Management_System.Migrations
{
    /// <inheritdoc />
    public partial class v311 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NewId",
                table: "Contacts",
                newName: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Contacts",
                newName: "NewId");
        }
    }
}
