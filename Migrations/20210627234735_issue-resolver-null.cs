using Microsoft.EntityFrameworkCore.Migrations;

namespace cumin_api.Migrations
{
    public partial class issueresolvernull : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Issue_Users_ResolverId",
                table: "Issue");

            migrationBuilder.AddForeignKey(
                name: "FK_Issue_Users_ResolverId",
                table: "Issue",
                column: "ResolverId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Issue_Users_ResolverId",
                table: "Issue");

            migrationBuilder.AddForeignKey(
                name: "FK_Issue_Users_ResolverId",
                table: "Issue",
                column: "ResolverId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
