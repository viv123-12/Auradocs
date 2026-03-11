using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace auradocs_api.Migrations
{
    /// <inheritdoc />
    public partial class ChangeInIndexesMadeNonUnique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Folder_uOwnerUSerId",
                table: "Folder");

            migrationBuilder.DropIndex(
                name: "IX_Folder_uParentFolderId",
                table: "Folder");

            migrationBuilder.DropIndex(
                name: "IX_Folder_uDocumentId",
                table: "DocumentVersion");

            migrationBuilder.DropIndex(
                name: "IX_Folder_uUpdatedBy",
                table: "DocumentVersion");

            migrationBuilder.DropIndex(
                name: "IX_DocumentFolder_uDocumentId",
                table: "DocumentFolder");

            migrationBuilder.DropIndex(
                name: "IX_DocumentFolder_uFolderId",
                table: "DocumentFolder");

            migrationBuilder.CreateIndex(
                name: "IX_Folder_uOwnerUSerId",
                table: "Folder",
                column: "uOwnerUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Folder_uParentFolderId",
                table: "Folder",
                column: "uParentFolderId");

            migrationBuilder.CreateIndex(
                name: "IX_Folder_uDocumentId",
                table: "DocumentVersion",
                column: "uDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_Folder_uUpdatedBy",
                table: "DocumentVersion",
                column: "uUpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentFolder_uDocumentId",
                table: "DocumentFolder",
                column: "uDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentFolder_uFolderId",
                table: "DocumentFolder",
                column: "uFolderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Folder_uOwnerUSerId",
                table: "Folder");

            migrationBuilder.DropIndex(
                name: "IX_Folder_uParentFolderId",
                table: "Folder");

            migrationBuilder.DropIndex(
                name: "IX_Folder_uDocumentId",
                table: "DocumentVersion");

            migrationBuilder.DropIndex(
                name: "IX_Folder_uUpdatedBy",
                table: "DocumentVersion");

            migrationBuilder.DropIndex(
                name: "IX_DocumentFolder_uDocumentId",
                table: "DocumentFolder");

            migrationBuilder.DropIndex(
                name: "IX_DocumentFolder_uFolderId",
                table: "DocumentFolder");

            migrationBuilder.CreateIndex(
                name: "IX_Folder_uOwnerUSerId",
                table: "Folder",
                column: "uOwnerUserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Folder_uParentFolderId",
                table: "Folder",
                column: "uParentFolderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Folder_uDocumentId",
                table: "DocumentVersion",
                column: "uDocumentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Folder_uUpdatedBy",
                table: "DocumentVersion",
                column: "uUpdatedBy",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DocumentFolder_uDocumentId",
                table: "DocumentFolder",
                column: "uDocumentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DocumentFolder_uFolderId",
                table: "DocumentFolder",
                column: "uFolderId",
                unique: true);
        }
    }
}
