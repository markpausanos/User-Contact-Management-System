using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace User_Contact_Management_System.Migrations
{
    /// <inheritdoc />
    public partial class v320 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContactNumber",
                table: "Contacts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmailAddress",
                table: "Contacts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Contacts",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContactNumber",
                table: "Contacts");

            migrationBuilder.DropColumn(
                name: "EmailAddress",
                table: "Contacts");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Contacts");
        }
    }
}
