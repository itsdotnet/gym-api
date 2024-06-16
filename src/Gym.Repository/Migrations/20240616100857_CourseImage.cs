using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gym.Repository.Migrations
{
    /// <inheritdoc />
    public partial class CourseImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "AttachmentId",
                table: "Courses",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Courses_AttachmentId",
                table: "Courses",
                column: "AttachmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Attachments_AttachmentId",
                table: "Courses",
                column: "AttachmentId",
                principalTable: "Attachments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Attachments_AttachmentId",
                table: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_Courses_AttachmentId",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "AttachmentId",
                table: "Courses");
        }
    }
}
