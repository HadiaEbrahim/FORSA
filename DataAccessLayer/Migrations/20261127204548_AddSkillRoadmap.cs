using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddSkillRoadmap : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SkillRoadmapSteps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SkillId = table.Column<int>(type: "int", nullable: false),
                    StepOrder = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ResourceUrl = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkillRoadmapSteps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SkillRoadmapSteps_Skills_SkillId",
                        column: x => x.SkillId,
                        principalTable: "Skills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentRoadmapProgresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    SkillRoadmapStepId = table.Column<int>(type: "int", nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentRoadmapProgresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentRoadmapProgresses_SkillRoadmapSteps_SkillRoadmapStepId",
                        column: x => x.SkillRoadmapStepId,
                        principalTable: "SkillRoadmapSteps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentRoadmapProgresses_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SkillRoadmapSteps_SkillId",
                table: "SkillRoadmapSteps",
                column: "SkillId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentRoadmapProgresses_SkillRoadmapStepId",
                table: "StudentRoadmapProgresses",
                column: "SkillRoadmapStepId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentRoadmapProgresses_StudentId_SkillRoadmapStepId",
                table: "StudentRoadmapProgresses",
                columns: new[] { "StudentId", "SkillRoadmapStepId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentRoadmapProgresses");

            migrationBuilder.DropTable(
                name: "SkillRoadmapSteps");
        }
    }
}
