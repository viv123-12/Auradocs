using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace auradocs_api.Migrations
{
    /// <inheritdoc />
    public partial class RemoveParentFolderIdMarkNotNull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Folder_uParentFolderId",
                table: "Folder");

            migrationBuilder.AlterColumn<int>(
                name: "uParentFolderId",
                table: "Folder",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "uParentFolderId",
                table: "Folder",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Folder_uParentFolderId",
                table: "Folder",
                column: "uParentFolderId",
                principalTable: "Folder",
                principalColumn: "uId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
