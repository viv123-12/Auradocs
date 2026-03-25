using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace auradocs_api.Migrations
{
    /// <inheritdoc />
    public partial class uDocumentTypestrFileUrlstrFileTypeToDocument : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "strFileType",
                table: "Document",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "strFileUrl",
                table: "Document",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "uDocumentType",
                table: "Document",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "strFileType",
                table: "Document");

            migrationBuilder.DropColumn(
                name: "strFileUrl",
                table: "Document");

            migrationBuilder.DropColumn(
                name: "uDocumentType",
                table: "Document");
        }
    }
}
