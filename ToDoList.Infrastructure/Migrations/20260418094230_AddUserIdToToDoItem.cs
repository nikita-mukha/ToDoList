using Microsoft.EntityFrameworkCore.Migrations;
using ToDoList.Infrastructure.Persistence;

#nullable disable

namespace ToDoList.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdToToDoItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Items",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Items");
        }
    }
}
