using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FortuneTeller.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUsersAndScopeWorries : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Clear orphaned rows so the FK constraint can be applied cleanly (dev only)
            migrationBuilder.Sql(@"DELETE FROM ""Worries"";");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Worries",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Username = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Worries_UserId",
                table: "Worries",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Worries_Users_UserId",
                table: "Worries",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Worries_Users_UserId",
                table: "Worries");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Worries_UserId",
                table: "Worries");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Worries");
        }
    }
}
