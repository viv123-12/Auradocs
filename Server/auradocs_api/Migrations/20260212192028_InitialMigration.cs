using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace auradocs_api.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "domainnamedropdown",
                columns: table => new
                {
                    uId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    domainName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_domainnamedropdown", x => x.uId);
                });

            migrationBuilder.CreateTable(
                name: "domainpracticeareadropdown",
                columns: table => new
                {
                    uId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    domainId = table.Column<int>(type: "integer", nullable: false),
                    practicearea = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_domainpracticeareadropdown", x => x.uId);
                    table.ForeignKey(
                        name: "FK_Dropdownoptionsgroup_ukey",
                        column: x => x.domainId,
                        principalTable: "domainpracticeareadropdown",
                        principalColumn: "uId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IndividualUsers",
                columns: table => new
                {
                    uUid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Guid = table.Column<Guid>(type: "uuid", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "character varying(254)", maxLength: 254, nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    PasswordSalt = table.Column<string>(type: "text", nullable: false),
                    UserRole = table.Column<int>(type: "integer", nullable: false),
                    PhoneNumber = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    AccountType = table.Column<string>(type: "text", nullable: false),
                    DomainType = table.Column<string>(type: "text", nullable: false),
                    PracticeArea = table.Column<string>(type: "text", nullable: false),
                    FullName = table.Column<string>(type: "character varying(254)", maxLength: 254, nullable: true),
                    ProfilePictureUrl = table.Column<string>(type: "text", nullable: true),
                    Address = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Description = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    IsUserActivated = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    dtLastLogin = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    dtAdded = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
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
                    Guid = table.Column<Guid>(type: "uuid", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "character varying(254)", maxLength: 254, nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    PasswordSalt = table.Column<string>(type: "text", nullable: false),
                    UserRole = table.Column<int>(type: "integer", nullable: false),
                    PhoneNumber = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    AccountType = table.Column<string>(type: "text", nullable: false),
                    DomainType = table.Column<string>(type: "text", nullable: false),
                    PracticeArea = table.Column<string>(type: "text", nullable: false),
                    FullName = table.Column<string>(type: "character varying(254)", maxLength: 254, nullable: true),
                    ProfilePictureUrl = table.Column<string>(type: "text", nullable: true),
                    Address = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Organization = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    jobTitle = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true),
                    Description = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    IsUserActivated = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    dtLastLogin = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    dtAdded = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationUsers", x => x.uUid);
                });

            migrationBuilder.InsertData(
                table: "domainnamedropdown",
                columns: new[] { "uId", "domainName" },
                values: new object[,]
                {
                    { 1, "IT" },
                    { 2, "Legal" },
                    { 3, "Healthcare" },
                    { 4, "Finance" },
                    { 5, "HR" },
                    { 6, "Marketing" },
                    { 7, "Consulting" }
                });

            migrationBuilder.InsertData(
                table: "domainpracticeareadropdown",
                columns: new[] { "uId", "practicearea", "domainId" },
                values: new object[,]
                {
                    { 1, "Web Development", 1 },
                    { 2, "Mobile Development", 1 },
                    { 3, "Cloud Engineering", 1 },
                    { 4, "DevOps", 1 },
                    { 5, "Data Engineering", 1 },
                    { 6, "Cybersecurity", 1 },
                    { 7, "AI / Machine Learning", 1 },
                    { 8, "QA / Testing", 1 },
                    { 9, "QA / Testing", 1 },
                    { 10, "Corporate Law", 2 },
                    { 11, "Criminal Defense", 2 },
                    { 12, "Civil Litigation", 2 },
                    { 13, "Family Law", 2 },
                    { 14, "Real Estate Law", 2 },
                    { 15, "Intellectual Property (IP)", 2 },
                    { 16, "Employment & Labor Law", 2 },
                    { 17, "Tax Law", 2 },
                    { 18, "Cardiology", 3 },
                    { 19, "Neurology", 3 },
                    { 20, "Orthopedics", 3 },
                    { 21, "Pediatrics", 3 },
                    { 22, "Oncology", 3 },
                    { 23, "General Medicine", 3 },
                    { 24, "Tax Advisory", 4 },
                    { 25, "Audit", 4 },
                    { 26, "Investment Management", 4 },
                    { 27, "Risk Management", 4 },
                    { 28, "Wealth Management", 4 },
                    { 29, "Recruitment", 5 },
                    { 30, "Learning & Development", 5 },
                    { 31, "Employee Relations", 5 },
                    { 32, "Compensation & Benefits", 5 },
                    { 33, "Digital Marketing", 6 },
                    { 34, "Content Marketing", 6 },
                    { 35, "Branding", 6 },
                    { 36, "SEO", 6 },
                    { 37, "Market Research", 6 },
                    { 38, "Business Strategy", 7 },
                    { 39, "Operations", 7 },
                    { 40, "IT Consulting", 7 },
                    { 41, "Supply Chain", 7 },
                    { 42, "Corporate Finance", 7 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_domainname_uId",
                table: "domainnamedropdown",
                column: "uId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_domainpracticeareadropdown_domainId",
                table: "domainpracticeareadropdown",
                column: "domainId");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "domainnamedropdown");

            migrationBuilder.DropTable(
                name: "domainpracticeareadropdown");

            migrationBuilder.DropTable(
                name: "IndividualUsers");

            migrationBuilder.DropTable(
                name: "OrganizationUsers");
        }
    }
}
