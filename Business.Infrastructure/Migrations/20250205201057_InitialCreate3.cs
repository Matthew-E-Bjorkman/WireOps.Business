using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Business.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Staffer_RoleId",
                table: "Staffer",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Staffer_Role_RoleId",
                table: "Staffer",
                column: "RoleId",
                principalTable: "Role",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Staffer_Role_RoleId",
                table: "Staffer");

            migrationBuilder.DropIndex(
                name: "IX_Staffer_RoleId",
                table: "Staffer");
        }
    }
}
