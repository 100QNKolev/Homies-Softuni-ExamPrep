using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Homies.Data.Migrations
{
    public partial class removedorganiser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Organiser",
                table: "Events");

            migrationBuilder.AlterColumn<string>(
                name: "OrganiserId",
                table: "Events",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "Id", "CreatedOn", "Description", "End", "Name", "OrganiserId", "Start", "TypeId" },
                values: new object[] { 5, new DateTime(2024, 2, 13, 9, 32, 26, 908, DateTimeKind.Local).AddTicks(7535), "Petrohanov salam", new DateTime(2024, 2, 13, 9, 32, 26, 908, DateTimeKind.Local).AddTicks(7564), "Ivan", "9371e050-e8eb-4236-81e8-3b251f8225c5", new DateTime(2024, 2, 13, 9, 32, 26, 908, DateTimeKind.Local).AddTicks(7562), 2 });

            migrationBuilder.CreateIndex(
                name: "IX_Events_OrganiserId",
                table: "Events",
                column: "OrganiserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_AspNetUsers_OrganiserId",
                table: "Events",
                column: "OrganiserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_AspNetUsers_OrganiserId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_OrganiserId",
                table: "Events");

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.AlterColumn<string>(
                name: "OrganiserId",
                table: "Events",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "Organiser",
                table: "Events",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
