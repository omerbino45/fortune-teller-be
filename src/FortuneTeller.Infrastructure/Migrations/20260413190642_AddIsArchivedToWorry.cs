using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FortuneTeller.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIsArchivedToWorry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsArchived",
                table: "Worries",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsArchived",
                table: "Worries");
        }
    }
}
