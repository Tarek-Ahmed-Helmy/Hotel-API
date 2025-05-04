using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class test : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Note",
                table: "Services");

            migrationBuilder.RenameColumn(
                name: "NameEN",
                table: "Statuses",
                newName: "NameEn");

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "RequestDetails",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Note",
                table: "RequestDetails");

            migrationBuilder.RenameColumn(
                name: "NameEn",
                table: "Statuses",
                newName: "NameEN");

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "Services",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
