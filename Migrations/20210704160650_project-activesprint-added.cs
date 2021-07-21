using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace cumin_api.Migrations
{
    public partial class projectactivesprintadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ActiveSprintId",
                table: "Sprints",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ActiveSprintId",
                table: "Projects",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ActiveSprintProject",
                columns: table => new
                {
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SprintId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActiveSprintProject", x => x.ProjectId);
                    table.UniqueConstraint("AK_ActiveSprintProject_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActiveSprintProject_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActiveSprintProject_Sprints_SprintId",
                        column: x => x.SprintId,
                        principalTable: "Sprints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ActiveSprintProject_SprintId",
                table: "ActiveSprintProject",
                column: "SprintId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActiveSprintProject");

            migrationBuilder.DropColumn(
                name: "ActiveSprintId",
                table: "Sprints");

            migrationBuilder.DropColumn(
                name: "ActiveSprintId",
                table: "Projects");
        }
    }
}
