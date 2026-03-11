using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace auradocs_api.Migrations
{
    /// <inheritdoc />
    public partial class ChangedDomainTypeAndPracticeAreaDataTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PracticeArea",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "PracticeArea",
                table: "Users",
                nullable: false,
                defaultValue: 1);


            migrationBuilder.DropColumn(
                name: "DomainType",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "DomainType",
                table: "Users",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateIndex(
                name: "IX_Users_DomainType",
                table: "Users",
                column: "DomainType");

            migrationBuilder.CreateIndex(
                name: "IX_Users_PracticeArea",
                table: "Users",
                column: "PracticeArea");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_uDomainType",
                table: "Users",
                column: "DomainType",
                principalTable: "domainnamedropdown",
                principalColumn: "uId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_uPracticeArea",
                table: "Users",
                column: "PracticeArea",
                principalTable: "domainpracticeareadropdown",
                principalColumn: "uId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_uDomainType",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_uPracticeArea",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_DomainType",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_PracticeArea",
                table: "Users");

            migrationBuilder.AlterColumn<string>(
                name: "PracticeArea",
                table: "Users",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "DomainType",
                table: "Users",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");
        }
    }
}
