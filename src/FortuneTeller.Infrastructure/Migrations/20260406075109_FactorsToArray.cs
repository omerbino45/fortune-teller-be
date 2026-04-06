using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FortuneTeller.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FactorsToArray : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // PostgreSQL requires an explicit USING clause when casting text → text[].
            // Existing null values stay null; existing text values are wrapped in a single-element array.
            migrationBuilder.Sql(
                """
                ALTER TABLE "Worries"
                ALTER COLUMN "Factors" TYPE text[]
                USING CASE
                    WHEN "Factors" IS NULL THEN NULL
                    ELSE ARRAY["Factors"]
                END;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Factors",
                table: "Worries",
                type: "text",
                nullable: true,
                oldClrType: typeof(List<string>),
                oldType: "text[]",
                oldNullable: true);
        }
    }
}
