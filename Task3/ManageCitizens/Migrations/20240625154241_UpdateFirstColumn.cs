using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ManageCitizens.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFirstColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Citizens",
                newName: "FirstName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "Citizens",
                newName: "Name");
        }
    }
}
