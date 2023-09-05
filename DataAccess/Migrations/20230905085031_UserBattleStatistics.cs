using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UserBattleStatistics : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserUnits_AspNetUsers_UserId",
                table: "UserUnits");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "UserUnits",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "Battles",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Defeats",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Victories",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserUnits_AspNetUsers_UserId",
                table: "UserUnits",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserUnits_AspNetUsers_UserId",
                table: "UserUnits");

            migrationBuilder.DropColumn(
                name: "Battles",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Defeats",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Victories",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "UserUnits",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserUnits_AspNetUsers_UserId",
                table: "UserUnits",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
