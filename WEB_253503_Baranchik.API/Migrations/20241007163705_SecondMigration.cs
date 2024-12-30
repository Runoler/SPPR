using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WEB_253503_Baranchik.API.Migrations
{
    /// <inheritdoc />
    public partial class SecondMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_RoomCategory_CategoryId",
                table: "Rooms");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoomCategory",
                table: "RoomCategory");

            migrationBuilder.RenameTable(
                name: "RoomCategory",
                newName: "RoomCategories");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoomCategories",
                table: "RoomCategories",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_RoomCategories_CategoryId",
                table: "Rooms",
                column: "CategoryId",
                principalTable: "RoomCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_RoomCategories_CategoryId",
                table: "Rooms");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoomCategories",
                table: "RoomCategories");

            migrationBuilder.RenameTable(
                name: "RoomCategories",
                newName: "RoomCategory");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoomCategory",
                table: "RoomCategory",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_RoomCategory_CategoryId",
                table: "Rooms",
                column: "CategoryId",
                principalTable: "RoomCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
