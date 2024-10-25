using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SourceControlApiV2.Migrations
{
    public partial class AddAllModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("8c13590b-9d70-4f1c-a9ff-fa0d655d16b9"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("ccaf2664-a70d-4443-92f1-f400d8bc9c74"));

            migrationBuilder.AddColumn<bool>(
                name: "isDeleted",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Flag to indicate if the user is deleted");

            migrationBuilder.CreateTable(
                name: "Repositories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Unique identifier for the repository"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "Name of the repository"),
                    Description = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false, comment: "Description of the repository"),
                    IsPublic = table.Column<bool>(type: "bit", nullable: false, comment: "Flag to indicate if the repository is public or private"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Owner of the repository"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, comment: "Flag to indicate if the repository is deleted")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Repositories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Repositories_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Issues",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Unique identifier for the issue"),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "Title of the issue"),
                    Description = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false, comment: "Description of the issue"),
                    Tags = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false, comment: "Comma separated list of the tags of the issue"),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "Status of the issue"),
                    RepositoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Repository that the issue belongs to"),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Creator of the issue"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, comment: "Flag to indicate if the issue is deleted")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Issues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Issues_AspNetUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Issues_Repositories_RepositoryId",
                        column: x => x.RepositoryId,
                        principalTable: "Repositories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PullRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Unique identifier for the pull request"),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "Title of the pull request"),
                    Description = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    IsResolved = table.Column<bool>(type: "bit", nullable: false, comment: "Flag to indicate if the pull request is resolved"),
                    RepositoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Repository that the pull request belongs to"),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "The creator of the pull request"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, comment: "Flag to indicate if the pull request is deleted")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PullRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PullRequests_AspNetUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PullRequests_Repositories_RepositoryId",
                        column: x => x.RepositoryId,
                        principalTable: "Repositories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RepositoryContributors",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Unique identifier for the repository contributor"),
                    RepositoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Unique identifier for the repository")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RepositoryContributors", x => new { x.UserId, x.RepositoryId });
                    table.ForeignKey(
                        name: "FK_RepositoryContributors_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RepositoryContributors_Repositories_RepositoryId",
                        column: x => x.RepositoryId,
                        principalTable: "Repositories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Commits",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Unique identifier for the commit"),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "Title of the commit"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RepositoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Repository that the commit belongs to"),
                    AuthorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Author of the commit"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, comment: "Flag to indicate if the commit is deleted"),
                    PullRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Pull request that the commit belongs to")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Commits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Commits_AspNetUsers_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Commits_PullRequests_PullRequestId",
                        column: x => x.PullRequestId,
                        principalTable: "PullRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Commits_Repositories_RepositoryId",
                        column: x => x.RepositoryId,
                        principalTable: "Repositories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Modifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Unique identifier for the modification"),
                    FileName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "Name of the file"),
                    FileDifferences = table.Column<string>(type: "nvarchar(1500)", maxLength: 1500, nullable: false, comment: "Differences in the file"),
                    modificationType = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "Type of modification"),
                    CommitId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Commit that the modification belongs to"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, comment: "Flag to indicate if the modification is deleted")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Modifications_Commits_CommitId",
                        column: x => x.CommitId,
                        principalTable: "Commits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { new Guid("5aa5e3d9-173d-4aea-9ea2-bc762da317c8"), "e6f30253-4741-4ab2-ac3b-cb938138c40c", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { new Guid("77a80b39-192b-4b76-985e-d00644598e9c"), "4e13db03-49ee-4800-a2f1-cec752442aa0", "User", "USER" });

            migrationBuilder.CreateIndex(
                name: "IX_Commits_AuthorId",
                table: "Commits",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Commits_PullRequestId",
                table: "Commits",
                column: "PullRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_Commits_RepositoryId",
                table: "Commits",
                column: "RepositoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Issues_CreatorId",
                table: "Issues",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Issues_RepositoryId",
                table: "Issues",
                column: "RepositoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Modifications_CommitId",
                table: "Modifications",
                column: "CommitId");

            migrationBuilder.CreateIndex(
                name: "IX_PullRequests_CreatorId",
                table: "PullRequests",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_PullRequests_RepositoryId",
                table: "PullRequests",
                column: "RepositoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Repositories_OwnerId",
                table: "Repositories",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_RepositoryContributors_RepositoryId",
                table: "RepositoryContributors",
                column: "RepositoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Issues");

            migrationBuilder.DropTable(
                name: "Modifications");

            migrationBuilder.DropTable(
                name: "RepositoryContributors");

            migrationBuilder.DropTable(
                name: "Commits");

            migrationBuilder.DropTable(
                name: "PullRequests");

            migrationBuilder.DropTable(
                name: "Repositories");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("5aa5e3d9-173d-4aea-9ea2-bc762da317c8"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("77a80b39-192b-4b76-985e-d00644598e9c"));

            migrationBuilder.DropColumn(
                name: "isDeleted",
                table: "AspNetUsers");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { new Guid("8c13590b-9d70-4f1c-a9ff-fa0d655d16b9"), "eea7d248-2e18-4c83-9c75-7893caaedc52", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { new Guid("ccaf2664-a70d-4443-92f1-f400d8bc9c74"), "09f3dea0-aa1b-4d08-b0bf-b4633f2e62a5", "User", "USER" });
        }
    }
}
