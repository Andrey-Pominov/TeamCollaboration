using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeamCollaboration.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Columns",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    BoardId = table.Column<Guid>(type: "uuid", nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    IsArchived = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Columns", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    BoardId = table.Column<Guid>(type: "uuid", nullable: false),
                    BoardColumnId = table.Column<Guid>(type: "uuid", nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    Assignee = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DueAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CompletedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    DisplayName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Columns_BoardId_SortOrder",
                table: "Columns",
                columns: new[] { "BoardId", "SortOrder" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_BoardId_BoardColumnId_SortOrder",
                table: "Tasks",
                columns: new[] { "BoardId", "BoardColumnId", "SortOrder" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Columns");

            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
