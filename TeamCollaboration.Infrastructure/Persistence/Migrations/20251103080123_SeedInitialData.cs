using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeamCollaboration.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var boardId = new Guid("11111111-1111-1111-1111-111111111111");
            var todoColumnId = new Guid("22222222-2222-2222-2222-222222222222");
            var inProgressColumnId = new Guid("33333333-3333-3333-3333-333333333333");
            var doneColumnId = new Guid("44444444-4444-4444-4444-444444444444");

            migrationBuilder.InsertData(
                table: "Columns",
                columns: new[] { "Id", "Name", "Description", "BoardId", "SortOrder", "IsArchived", "CreatedAtUtc" },
                values: new object[,]
                {
                    { todoColumnId, "To Do", null, boardId, 0, false, DateTime.UtcNow },
                    { inProgressColumnId, "In Progress", null, boardId, 1, false, DateTime.UtcNow },
                    { doneColumnId, "Done", null, boardId, 2, false, DateTime.UtcNow }
                }
            );

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Title", "Description", "Status", "BoardId", "BoardColumnId", "SortOrder", "Assignee", "CreatedAtUtc", "DueAtUtc", "CompletedAtUtc" },
                values: new object[,]
                {
                    { new Guid("55555555-5555-5555-5555-555555555555"), "Set up project", "Initialize solution and projects", 0, boardId, todoColumnId, 0, "Alex", DateTime.UtcNow, DateTime.UtcNow.AddDays(2), null },
                    { new Guid("66666666-6666-6666-6666-666666666666"), "Design DB schema", "Model tasks and columns", 0, boardId, todoColumnId, 1, "Maria", DateTime.UtcNow, DateTime.UtcNow.AddDays(3), null },
                    { new Guid("77777777-7777-7777-7777-777777777777"), "Implement Kanban UI", "Build columns and cards", 0, boardId, todoColumnId, 2, null, DateTime.UtcNow, null, null }
                }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValues: new object[]
                {
                    new Guid("55555555-5555-5555-5555-555555555555"),
                    new Guid("66666666-6666-6666-6666-666666666666"),
                    new Guid("77777777-7777-7777-7777-777777777777")
                }
            );

            migrationBuilder.DeleteData(
                table: "Columns",
                keyColumn: "Id",
                keyValues: new object[]
                {
                    new Guid("22222222-2222-2222-2222-222222222222"),
                    new Guid("33333333-3333-3333-3333-333333333333"),
                    new Guid("44444444-4444-4444-4444-444444444444")
                }
            );
        }
    }
}
