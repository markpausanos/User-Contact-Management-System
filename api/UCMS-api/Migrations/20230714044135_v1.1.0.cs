using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace User_Contact_Management_System.Migrations
{
    /// <inheritdoc />
    public partial class v110 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TABLE Users ADD [Password] NVARCHAR(MAX) NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
