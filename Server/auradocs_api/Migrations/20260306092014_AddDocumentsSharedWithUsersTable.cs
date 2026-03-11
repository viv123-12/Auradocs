using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace auradocs_api.Migrations
{
    /// <inheritdoc />
    public partial class AddDocumentsSharedWithUsersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DocumentsSharedWithUsers",
                columns: table => new
                {
                    uId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    uSharedDocumentId = table.Column<int>(type: "integer", nullable: false),
                    uSharedWith = table.Column<int>(type: "integer", nullable: false),
                    uSharedBy = table.Column<int>(type: "integer", nullable: false),
                    uAccessgiven = table.Column<int>(type: "integer", nullable: false),
                    dtAccessGivenOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentsSharedWithUsers", x => x.uId);
                    table.ForeignKey(
                        name: "FK_DocumentsSharedWithUser_uSharedBy",
                        column: x => x.uSharedBy,
                        principalTable: "Users",
                        principalColumn: "uUid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DocumentsSharedWithUser_uSharedDocumentId",
                        column: x => x.uSharedDocumentId,
                        principalTable: "Document",
                        principalColumn: "uId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DocumentsSharedWithUser_uSharedWith",
                        column: x => x.uSharedWith,
                        principalTable: "Users",
                        principalColumn: "uUid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DocumentsSharedWithUser_uSharedBy",
                table: "DocumentsSharedWithUsers",
                column: "uSharedBy");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentsSharedWithUser_uSharedWith",
                table: "DocumentsSharedWithUsers",
                column: "uSharedWith");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentsSharedWithUsers_uSharedDocumentId",
                table: "DocumentsSharedWithUsers",
                column: "uSharedDocumentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DocumentsSharedWithUsers");
        }
    }
}
