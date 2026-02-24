using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ComplyFlow.API.Migrations
{
    /// <inheritdoc />
    public partial class AddSubTaskAssignments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TaskItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Priority = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TaskType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AssignedToUserId = table.Column<int>(type: "int", nullable: true),
                    AssignedToGroupId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskItems_Groups_AssignedToGroupId",
                        column: x => x.AssignedToGroupId,
                        principalTable: "Groups",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TaskItems_Users_AssignedToUserId",
                        column: x => x.AssignedToUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SubTasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AssignedToUserId = table.Column<int>(type: "int", nullable: true),
                    AssignedToGroupId = table.Column<int>(type: "int", nullable: true),
                    TaskItemId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubTasks_Groups_AssignedToGroupId",
                        column: x => x.AssignedToGroupId,
                        principalTable: "Groups",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SubTasks_TaskItems_TaskItemId",
                        column: x => x.TaskItemId,
                        principalTable: "TaskItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubTasks_Users_AssignedToUserId",
                        column: x => x.AssignedToUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Groups",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Uyum Ekibi" },
                    { 2, "KVKK Kurulu" },
                    { 3, "Hukuk Departmanı" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "FullName", "Role" },
                values: new object[,]
                {
                    { 1, "Ahmet Yılmaz", "Avukat" },
                    { 2, "Ayşe Demir", "Uyum Uzmanı" },
                    { 3, "Mehmet Kaya", "Stajyer" },
                    { 4, "Zeynep Çelik", "Yönetici" }
                });

            migrationBuilder.InsertData(
                table: "TaskItems",
                columns: new[] { "Id", "AssignedToGroupId", "AssignedToUserId", "Description", "DueDate", "Priority", "Status", "TaskType", "Title" },
                values: new object[,]
                {
                    { 1, 3, 1, "Yeni tedarikçi sözleşmesinin incelenmesi.", new DateTime(2026, 2, 27, 11, 6, 11, 460, DateTimeKind.Local).AddTicks(981), "Yüksek", "Beklemede", "", "Sözleşme İncelemesi" },
                    { 2, 2, 2, "Web sitesindeki aydınlatma metninin güncellenmesi.", new DateTime(2026, 3, 3, 11, 6, 11, 465, DateTimeKind.Local).AddTicks(1447), "Orta", "Devam Ediyor", "", "KVKK Aydınlatma Metni Güncellemesi" },
                    { 3, 1, null, "Personel için yıllık uyum eğitimi sunumunun hazırlanması.", new DateTime(2026, 2, 22, 11, 6, 11, 465, DateTimeKind.Local).AddTicks(1460), "Düşük", "Tamamlandı", "", "Uyum Eğitimi Hazırlığı" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_SubTasks_AssignedToGroupId",
                table: "SubTasks",
                column: "AssignedToGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_SubTasks_AssignedToUserId",
                table: "SubTasks",
                column: "AssignedToUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SubTasks_TaskItemId",
                table: "SubTasks",
                column: "TaskItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskItems_AssignedToGroupId",
                table: "TaskItems",
                column: "AssignedToGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskItems_AssignedToUserId",
                table: "TaskItems",
                column: "AssignedToUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SubTasks");

            migrationBuilder.DropTable(
                name: "TaskItems");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
