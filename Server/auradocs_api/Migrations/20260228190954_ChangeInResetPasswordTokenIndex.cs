using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace auradocs_api.Migrations
{
    /// <inheritdoc />
    public partial class ChangeInResetPasswordTokenIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ResetPasswordToken_strUserId",
                table: "ResetPasswordToken");

            migrationBuilder.CreateIndex(
                name: "IX_ResetPasswordToken_strUserId",
                table: "ResetPasswordToken",
                column: "strUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ResetPasswordToken_strUserId",
                table: "ResetPasswordToken");

            migrationBuilder.CreateIndex(
                name: "IX_ResetPasswordToken_strUserId",
                table: "ResetPasswordToken",
                column: "strUserId",
                unique: true);
        }
    }
}
