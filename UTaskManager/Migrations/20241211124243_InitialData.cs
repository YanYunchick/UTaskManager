using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UTaskManager.Migrations
{
    /// <inheritdoc />
    public partial class InitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "UserTasks",
                columns: new[] { "UserTaskId", "CreatedAt", "Deadline", "Description", "Priority", "Status", "Title", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("3d490a70-94ce-4d15-9494-5248280c2ce3"), new DateTime(2024, 12, 11, 15, 42, 43, 10, DateTimeKind.Local).AddTicks(5515), new DateTime(2025, 12, 25, 15, 30, 0, 0, DateTimeKind.Unspecified), "TestDescription2", 2, 0, "TestTitle2", new DateTime(2024, 12, 11, 15, 42, 43, 10, DateTimeKind.Local).AddTicks(5517) },
                    { new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870"), new DateTime(2024, 12, 11, 15, 42, 43, 8, DateTimeKind.Local).AddTicks(8593), new DateTime(2024, 12, 25, 15, 30, 0, 0, DateTimeKind.Unspecified), "TestDescription1", 1, 1, "TestTitle1", new DateTime(2024, 12, 11, 15, 42, 43, 10, DateTimeKind.Local).AddTicks(5310) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserTasks",
                keyColumn: "UserTaskId",
                keyValue: new Guid("3d490a70-94ce-4d15-9494-5248280c2ce3"));

            migrationBuilder.DeleteData(
                table: "UserTasks",
                keyColumn: "UserTaskId",
                keyValue: new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870"));
        }
    }
}
