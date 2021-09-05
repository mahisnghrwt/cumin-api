using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace cumin_api.Migrations
{
    public partial class epicissuerelationshipadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>("EpicId", "Issues", type: "int", nullable: true);

            //migrationBuilder.CreateTable(
            //    name: "Issues",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
            //        Title = table.Column<string>(type: "longtext", nullable: true)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        Description = table.Column<string>(type: "longtext", nullable: true)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
            //        Type = table.Column<string>(type: "longtext", nullable: true)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        Status = table.Column<string>(type: "longtext", nullable: true)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        ProjectId = table.Column<int>(type: "int", nullable: false),
            //        ReporterId = table.Column<int>(type: "int", nullable: false),
            //        ResolverId = table.Column<int>(type: "int", nullable: true),
            //        SprintId = table.Column<int>(type: "int", nullable: true),
            //        EpicId = table.Column<int>(type: "int", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Issues", x => x.Id);
            //    })
            //    .Annotation("MySql:CharSet", "utf8mb4");

            //migrationBuilder.CreateTable(
            //    name: "Path",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
            //        FromEpicId = table.Column<int>(type: "int", nullable: false),
            //        ToEpicId = table.Column<int>(type: "int", nullable: false),
            //        ProjectId = table.Column<int>(type: "int", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Path", x => x.Id);
            //    })
            //    .Annotation("MySql:CharSet", "utf8mb4");

            //migrationBuilder.CreateTable(
            //    name: "Epic",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
            //        Title = table.Column<string>(type: "longtext", nullable: true)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        StartDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
            //        EndDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
            //        Row = table.Column<int>(type: "int", nullable: false),
            //        ProjectId = table.Column<int>(type: "int", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Epic", x => x.Id);
            //        table.UniqueConstraint("AK_Epic_ProjectId_Row", x => new { x.ProjectId, x.Row });
            //    })
            //    .Annotation("MySql:CharSet", "utf8mb4");

            //migrationBuilder.CreateTable(
            //    name: "ProjectInvitations",
            //    columns: table => new
            //    {
            //        InviterId = table.Column<int>(type: "int", nullable: false),
            //        InviteeId = table.Column<int>(type: "int", nullable: false),
            //        ProjectId = table.Column<int>(type: "int", nullable: false),
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
            //        InvitedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_ProjectInvitations", x => new { x.InviteeId, x.InviterId, x.ProjectId });
            //        table.UniqueConstraint("AK_ProjectInvitations_Id", x => x.Id);
            //    })
            //    .Annotation("MySql:CharSet", "utf8mb4");

            //migrationBuilder.CreateTable(
            //    name: "Sprints",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
            //        Title = table.Column<string>(type: "longtext", nullable: true)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        ProjectId = table.Column<int>(type: "int", nullable: false),
            //        CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Sprints", x => x.Id);
            //    })
            //    .Annotation("MySql:CharSet", "utf8mb4");

            //migrationBuilder.CreateTable(
            //    name: "Projects",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
            //        Name = table.Column<string>(type: "longtext", nullable: true)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        StartDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
            //        ActiveSprintId = table.Column<int>(type: "int", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Projects", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_Projects_Sprints_ActiveSprintId",
            //            column: x => x.ActiveSprintId,
            //            principalTable: "Sprints",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //    })
            //    .Annotation("MySql:CharSet", "utf8mb4");

            //migrationBuilder.CreateTable(
            //    name: "Users",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
            //        Username = table.Column<string>(type: "varchar(255)", nullable: false)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        Password = table.Column<string>(type: "longtext", nullable: true)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        ActiveProjectId = table.Column<int>(type: "int", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Users", x => x.Id);
            //        table.UniqueConstraint("AK_Users_Username", x => x.Username);
            //        table.ForeignKey(
            //            name: "FK_Users_Projects_ActiveProjectId",
            //            column: x => x.ActiveProjectId,
            //            principalTable: "Projects",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //    })
            //    .Annotation("MySql:CharSet", "utf8mb4");

            //migrationBuilder.CreateTable(
            //    name: "UserProjects",
            //    columns: table => new
            //    {
            //        UserId = table.Column<int>(type: "int", nullable: false),
            //        ProjectId = table.Column<int>(type: "int", nullable: false),
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_UserProjects", x => new { x.UserId, x.ProjectId });
            //        table.UniqueConstraint("AK_UserProjects_Id", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_UserProjects_Projects_ProjectId",
            //            column: x => x.ProjectId,
            //            principalTable: "Projects",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_UserProjects_Users_UserId",
            //            column: x => x.UserId,
            //            principalTable: "Users",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    })
            //    .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Issues_EpicId",
                table: "Issues",
                column: "EpicId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Issues_ProjectId",
            //    table: "Issues",
            //    column: "ProjectId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Issues_ReporterId",
            //    table: "Issues",
            //    column: "ReporterId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Issues_ResolverId",
            //    table: "Issues",
            //    column: "ResolverId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Issues_SprintId",
            //    table: "Issues",
            //    column: "SprintId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Path_FromEpicId",
            //    table: "Path",
            //    column: "FromEpicId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Path_ProjectId",
            //    table: "Path",
            //    column: "ProjectId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Path_ToEpicId",
            //    table: "Path",
            //    column: "ToEpicId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_ProjectInvitations_InviterId",
            //    table: "ProjectInvitations",
            //    column: "InviterId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_ProjectInvitations_ProjectId",
            //    table: "ProjectInvitations",
            //    column: "ProjectId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Projects_ActiveSprintId",
            //    table: "Projects",
            //    column: "ActiveSprintId",
            //    unique: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_Sprints_ProjectId",
            //    table: "Sprints",
            //    column: "ProjectId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_UserProjects_ProjectId",
            //    table: "UserProjects",
            //    column: "ProjectId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Users_ActiveProjectId",
            //    table: "Users",
            //    column: "ActiveProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Issues_Epic_EpicId",
                table: "Issues",
                column: "EpicId",
                principalTable: "Epic",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Issues_Projects_ProjectId",
            //    table: "Issues",
            //    column: "ProjectId",
            //    principalTable: "Projects",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Issues_Sprints_SprintId",
            //    table: "Issues",
            //    column: "SprintId",
            //    principalTable: "Sprints",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Restrict);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Issues_Users_ReporterId",
            //    table: "Issues",
            //    column: "ReporterId",
            //    principalTable: "Users",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Issues_Users_ResolverId",
            //    table: "Issues",
            //    column: "ResolverId",
            //    principalTable: "Users",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Restrict);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Path_Epic_FromEpicId",
            //    table: "Path",
            //    column: "FromEpicId",
            //    principalTable: "Epic",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Path_Epic_ToEpicId",
            //    table: "Path",
            //    column: "ToEpicId",
            //    principalTable: "Epic",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Path_Projects_ProjectId",
            //    table: "Path",
            //    column: "ProjectId",
            //    principalTable: "Projects",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Epic_Projects_ProjectId",
            //    table: "Epic",
            //    column: "ProjectId",
            //    principalTable: "Projects",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_ProjectInvitations_Projects_ProjectId",
            //    table: "ProjectInvitations",
            //    column: "ProjectId",
            //    principalTable: "Projects",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "invitee-projectinvitation",
            //    table: "ProjectInvitations",
            //    column: "InviteeId",
            //    principalTable: "Users",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "inviter-projectinvitation",
            //    table: "ProjectInvitations",
            //    column: "InviterId",
            //    principalTable: "Users",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Sprints_Projects_ProjectId",
            //    table: "Sprints",
            //    column: "ProjectId",
            //    principalTable: "Projects",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sprints_Projects_ProjectId",
                table: "Sprints");

            migrationBuilder.DropTable(
                name: "Issues");

            migrationBuilder.DropTable(
                name: "Path");

            migrationBuilder.DropTable(
                name: "ProjectInvitations");

            migrationBuilder.DropTable(
                name: "UserProjects");

            migrationBuilder.DropTable(
                name: "Epic");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "Sprints");
        }
    }
}
