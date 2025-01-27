using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UTaskManager.Migrations
{
    /// <inheritdoc />
    public partial class AddedRolesToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3472791a-25b5-4804-ad2d-ad75ae671c57", null, "User", "USER" },
                    { "fd9e896e-29dd-4d9a-89dd-853f462ff988", null, "Administrator", "ADMINISTRATOR" }
                });

            migrationBuilder.UpdateData(
                table: "UserTasks",
                keyColumn: "UserTaskId",
                keyValue: new Guid("3d490a70-94ce-4d15-9494-5248280c2ce3"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 1, 9, 16, 22, 17, 614, DateTimeKind.Local).AddTicks(9281), new DateTime(2025, 1, 9, 16, 22, 17, 614, DateTimeKind.Local).AddTicks(9281) });

            migrationBuilder.UpdateData(
                table: "UserTasks",
                keyColumn: "UserTaskId",
                keyValue: new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 1, 9, 16, 22, 17, 614, DateTimeKind.Local).AddTicks(9256), new DateTime(2025, 1, 9, 16, 22, 17, 614, DateTimeKind.Local).AddTicks(9276) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3472791a-25b5-4804-ad2d-ad75ae671c57");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fd9e896e-29dd-4d9a-89dd-853f462ff988");

            migrationBuilder.UpdateData(
                table: "UserTasks",
                keyColumn: "UserTaskId",
                keyValue: new Guid("3d490a70-94ce-4d15-9494-5248280c2ce3"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 1, 9, 16, 14, 0, 813, DateTimeKind.Local).AddTicks(4650), new DateTime(2025, 1, 9, 16, 14, 0, 813, DateTimeKind.Local).AddTicks(4650) });

            migrationBuilder.UpdateData(
                table: "UserTasks",
                keyColumn: "UserTaskId",
                keyValue: new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 1, 9, 16, 14, 0, 813, DateTimeKind.Local).AddTicks(4625), new DateTime(2025, 1, 9, 16, 14, 0, 813, DateTimeKind.Local).AddTicks(4645) });
        }
    }
}
