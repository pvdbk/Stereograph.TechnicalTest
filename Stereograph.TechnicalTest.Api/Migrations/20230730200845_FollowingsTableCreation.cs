using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stereograph.TechnicalTest.Api.Migrations
{
    /// <inheritdoc />
    public partial class FollowingsTableCreation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Followings",
                columns: table => new
                {
                    FollowerId = table.Column<int>(type: "INTEGER", nullable: false),
                    FollowedPersonId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Followings", x => new { x.FollowedPersonId, x.FollowerId });
                    table.ForeignKey(
                        name: "FK_Followings_Persons_FollowedPersonId",
                        column: x => x.FollowedPersonId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Followings_Persons_FollowerId",
                        column: x => x.FollowerId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Followings_FollowerId_FollowedPersonId",
                table: "Followings",
                columns: new[] { "FollowerId", "FollowedPersonId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Followings");
        }
    }
}
