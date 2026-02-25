using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComplyFlow.API.Migrations
{
    /// <inheritdoc />
    public partial class AddSubTaskDescriptionAndDueDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "SubTasks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DueDate",
                table: "SubTasks",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "SubTasks");

            migrationBuilder.DropColumn(
                name: "DueDate",
                table: "SubTasks");
        }
    }
}
