using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolProject.Infrustructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedLocalizationForStudentAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Address",
                table: "students",
                newName: "AddressEn");

            migrationBuilder.AddColumn<string>(
                name: "AddressAr",
                table: "students",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddressAr",
                table: "students");

            migrationBuilder.RenameColumn(
                name: "AddressEn",
                table: "students",
                newName: "Address");
        }
    }
}
