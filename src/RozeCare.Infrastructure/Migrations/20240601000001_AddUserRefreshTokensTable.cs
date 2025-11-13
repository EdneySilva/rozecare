using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RozeCare.Infrastructure.Migrations
{
    public partial class AddUserRefreshTokensTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserRefreshTokens_UserId_Token",
                table: "UserRefreshTokens");

            migrationBuilder.AlterColumn<string>(
                name: "Token",
                table: "UserRefreshTokens",
                type: "character varying(2048)",
                maxLength: 2048,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<bool>(
                name: "IsRevoked",
                table: "UserRefreshTokens",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.CreateIndex(
                name: "IX_UserRefreshTokens_UserId",
                table: "UserRefreshTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRefreshTokens_Token",
                table: "UserRefreshTokens",
                column: "Token",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserRefreshTokens_Token",
                table: "UserRefreshTokens");

            migrationBuilder.DropIndex(
                name: "IX_UserRefreshTokens_UserId",
                table: "UserRefreshTokens");

            migrationBuilder.AlterColumn<bool>(
                name: "IsRevoked",
                table: "UserRefreshTokens",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "Token",
                table: "UserRefreshTokens",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(2048)",
                oldMaxLength: 2048);

            migrationBuilder.CreateIndex(
                name: "IX_UserRefreshTokens_UserId_Token",
                table: "UserRefreshTokens",
                columns: new[] { "UserId", "Token" },
                unique: true);
        }
    }
}
