using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace auradocs_api.Migrations
{
    /// <inheritdoc />
    public partial class AddedDocumentSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IndividualUsers");

            migrationBuilder.DropTable(
                name: "OrganizationUsers");

            migrationBuilder.CreateTable(
                name: "ResetPasswordToken",
                columns: table => new
                {
                    uId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    strGuid = table.Column<string>(type: "text", nullable: false),
                    strUserId = table.Column<string>(type: "text", nullable: false),
                    strToken = table.Column<string>(type: "text", nullable: false),
                    dtExpiredAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    boolIsVerified = table.Column<bool>(type: "boolean", nullable: false),
                    boolIsUsed = table.Column<bool>(type: "boolean", nullable: false),
                    dtCreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResetPasswordToken", x => x.uId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    uUid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Guid = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "character varying(254)", maxLength: 254, nullable: false),
                    Password = table.Column<string>(type: "text", nullable: true),
                    PasswordSalt = table.Column<string>(type: "text", nullable: true),
                    UserRole = table.Column<int>(type: "integer", nullable: false),
                    PhoneNumber = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    AccountType = table.Column<string>(type: "text", nullable: false),
                    DomainType = table.Column<string>(type: "text", nullable: false),
                    PracticeArea = table.Column<string>(type: "text", nullable: false),
                    FullName = table.Column<string>(type: "character varying(254)", maxLength: 254, nullable: true),
                    ProfilePictureUrl = table.Column<string>(type: "text", nullable: true),
                    Address = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Description = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    strOrganizationName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    strJobTitle = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    IsUserActivated = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    dtLastLogin = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    dtAdded = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.uUid);
                });

            migrationBuilder.CreateTable(
                name: "Document",
                columns: table => new
                {
                    uId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    strGuid = table.Column<string>(type: "text", nullable: false),
                    strTitle = table.Column<string>(type: "text", nullable: false),
                    strContent = table.Column<string>(type: "text", nullable: false),
                    uStatusId = table.Column<int>(type: "integer", nullable: false),
                    uCurrentVersionId = table.Column<int>(type: "integer", nullable: false),
                    uCreatedBy = table.Column<int>(type: "integer", nullable: false),
                    uOwnerUserId = table.Column<int>(type: "integer", nullable: false),
                    boolIsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    dtUpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    dtCreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Document", x => x.uId);
                    table.ForeignKey(
                        name: "FK_Folder_uCreatedBy",
                        column: x => x.uCreatedBy,
                        principalTable: "Users",
                        principalColumn: "uUid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Folder_uOwnerUserId",
                        column: x => x.uOwnerUserId,
                        principalTable: "Users",
                        principalColumn: "uUid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Folder",
                columns: table => new
                {
                    uId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    strGuid = table.Column<string>(type: "text", nullable: false),
                    strTitle = table.Column<string>(type: "text", nullable: false),
                    uParentFolderId = table.Column<int>(type: "integer", nullable: false),
                    uOwnerUserId = table.Column<int>(type: "integer", nullable: false),
                    boolIsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    uCreatedBy = table.Column<int>(type: "integer", nullable: false),
                    dtCreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Folder", x => x.uId);
                    table.ForeignKey(
                        name: "FK_Folder_uCreatedBy",
                        column: x => x.uCreatedBy,
                        principalTable: "Users",
                        principalColumn: "uUid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Folder_uOwnerUserId",
                        column: x => x.uOwnerUserId,
                        principalTable: "Users",
                        principalColumn: "uUid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DocumentVersion",
                columns: table => new
                {
                    uId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    strGuid = table.Column<string>(type: "text", nullable: false),
                    uVersion = table.Column<int>(type: "integer", nullable: false),
                    uDocumentId = table.Column<int>(type: "integer", nullable: false),
                    strContent = table.Column<string>(type: "text", nullable: false),
                    uUpdatedBy = table.Column<int>(type: "integer", nullable: false),
                    dtUpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentVersion", x => x.uId);
                    table.ForeignKey(
                        name: "FK_DocumentVersion_uDocumentId",
                        column: x => x.uDocumentId,
                        principalTable: "Document",
                        principalColumn: "uId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DocumentVersion_uUpdatedBy",
                        column: x => x.uUpdatedBy,
                        principalTable: "Users",
                        principalColumn: "uUid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DocumentFolder",
                columns: table => new
                {
                    uId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    strGuid = table.Column<string>(type: "text", nullable: false),
                    uDocumentId = table.Column<int>(type: "integer", nullable: false),
                    uFolderId = table.Column<int>(type: "integer", nullable: false),
                    dtCreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentFolder", x => x.uId);
                    table.ForeignKey(
                        name: "FK_DocumentFolder_uDocumentId",
                        column: x => x.uDocumentId,
                        principalTable: "Document",
                        principalColumn: "uId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DocumentFolder_uFolderId",
                        column: x => x.uFolderId,
                        principalTable: "Folder",
                        principalColumn: "uId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Document_boolIsDeleted",
                table: "Document",
                column: "uStatusId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Document_uCreatedBy",
                table: "Document",
                column: "uCreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Document_uOwnerUserId",
                table: "Document",
                column: "uOwnerUserId");

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
                name: "IX_Folder_uCreatedBy",
                table: "Folder",
                column: "uCreatedBy");

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
                name: "IX_ResetPasswordToken_strUserId",
                table: "ResetPasswordToken",
                column: "strUserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_PhoneNumber",
                table: "Users",
                column: "PhoneNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DocumentFolder");

            migrationBuilder.DropTable(
                name: "DocumentVersion");

            migrationBuilder.DropTable(
                name: "ResetPasswordToken");

            migrationBuilder.DropTable(
                name: "Folder");

            migrationBuilder.DropTable(
                name: "Document");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.CreateTable(
                name: "IndividualUsers",
                columns: table => new
                {
                    uUid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsUserActivated = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    dtAdded = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    dtLastLogin = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Address = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Description = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(254)", maxLength: 254, nullable: false),
                    FullName = table.Column<string>(type: "character varying(254)", maxLength: 254, nullable: true),
                    Guid = table.Column<Guid>(type: "uuid", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    PasswordSalt = table.Column<string>(type: "text", nullable: false),
                    ProfilePictureUrl = table.Column<string>(type: "text", nullable: true),
                    UserRole = table.Column<int>(type: "integer", nullable: false),
                    PhoneNumber = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    AccountType = table.Column<string>(type: "text", nullable: false),
                    DomainType = table.Column<string>(type: "text", nullable: false),
                    PracticeArea = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IndividualUsers", x => x.uUid);
                });

            migrationBuilder.CreateTable(
                name: "OrganizationUsers",
                columns: table => new
                {
                    uUid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsUserActivated = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    dtAdded = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    dtLastLogin = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Address = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Description = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    FullName = table.Column<string>(type: "character varying(254)", maxLength: 254, nullable: true),
                    Guid = table.Column<Guid>(type: "uuid", maxLength: 100, nullable: false),
                    jobTitle = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true),
                    Email = table.Column<string>(type: "character varying(254)", maxLength: 254, nullable: false),
                    Organization = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    Password = table.Column<string>(type: "text", nullable: false),
                    PasswordSalt = table.Column<string>(type: "text", nullable: false),
                    ProfilePictureUrl = table.Column<string>(type: "text", nullable: true),
                    UserRole = table.Column<int>(type: "integer", nullable: false),
                    PhoneNumber = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    AccountType = table.Column<string>(type: "text", nullable: false),
                    DomainType = table.Column<string>(type: "text", nullable: false),
                    PracticeArea = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationUsers", x => x.uUid);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IndividualUser_Email",
                table: "IndividualUsers",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IndividualUser_PhoneNumber",
                table: "IndividualUsers",
                column: "PhoneNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationUser_OrganizationId",
                table: "OrganizationUsers",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationUser_PhoneNumber",
                table: "OrganizationUsers",
                column: "PhoneNumber",
                unique: true);
        }
    }
}
