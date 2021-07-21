using Microsoft.EntityFrameworkCore.Migrations;

namespace cumin_api.Migrations
{
    public partial class issues : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Issue_Projects_ProjectId",
                table: "Issue");

            migrationBuilder.DropForeignKey(
                name: "FK_Issue_Users_ReporterId",
                table: "Issue");

            migrationBuilder.DropForeignKey(
                name: "FK_Issue_Users_ResolverId",
                table: "Issue");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Issue",
                table: "Issue");

            migrationBuilder.RenameTable(
                name: "Issue",
                newName: "Issues");

            migrationBuilder.RenameIndex(
                name: "IX_Issue_ResolverId",
                table: "Issues",
                newName: "IX_Issues_ResolverId");

            migrationBuilder.RenameIndex(
                name: "IX_Issue_ReporterId",
                table: "Issues",
                newName: "IX_Issues_ReporterId");

            migrationBuilder.RenameIndex(
                name: "IX_Issue_ProjectId",
                table: "Issues",
                newName: "IX_Issues_ProjectId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Issues",
                table: "Issues",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Issues_Projects_ProjectId",
                table: "Issues",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Issues_Users_ReporterId",
                table: "Issues",
                column: "ReporterId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Issues_Users_ResolverId",
                table: "Issues",
                column: "ResolverId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Issues_Projects_ProjectId",
                table: "Issues");

            migrationBuilder.DropForeignKey(
                name: "FK_Issues_Users_ReporterId",
                table: "Issues");

            migrationBuilder.DropForeignKey(
                name: "FK_Issues_Users_ResolverId",
                table: "Issues");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Issues",
                table: "Issues");

            migrationBuilder.RenameTable(
                name: "Issues",
                newName: "Issue");

            migrationBuilder.RenameIndex(
                name: "IX_Issues_ResolverId",
                table: "Issue",
                newName: "IX_Issue_ResolverId");

            migrationBuilder.RenameIndex(
                name: "IX_Issues_ReporterId",
                table: "Issue",
                newName: "IX_Issue_ReporterId");

            migrationBuilder.RenameIndex(
                name: "IX_Issues_ProjectId",
                table: "Issue",
                newName: "IX_Issue_ProjectId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Issue",
                table: "Issue",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Issue_Projects_ProjectId",
                table: "Issue",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Issue_Users_ReporterId",
                table: "Issue",
                column: "ReporterId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Issue_Users_ResolverId",
                table: "Issue",
                column: "ResolverId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
