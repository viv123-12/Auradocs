using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace auradocs_api.Migrations
{
    /// <inheritdoc />
    public partial class changeIndexUniqueToNonUnique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Document_boolIsDeleted",
                table: "Document");

            migrationBuilder.CreateIndex(
                name: "IX_Document_boolIsDeleted",
                table: "Document",
                column: "uStatusId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Document_boolIsDeleted",
                table: "Document");

            migrationBuilder.CreateIndex(
                name: "IX_Document_boolIsDeleted",
                table: "Document",
                column: "uStatusId",
                unique: true);
        }
    }
}
