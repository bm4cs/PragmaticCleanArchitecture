using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookify.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RoleBasedAuthz : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_permission_role_role_id",
                table: "permission");

            migrationBuilder.DropPrimaryKey(
                name: "pk_role",
                table: "role");

            migrationBuilder.DropPrimaryKey(
                name: "pk_permission",
                table: "permission");

            migrationBuilder.DropIndex(
                name: "ix_permission_role_id",
                table: "permission");

            migrationBuilder.DropColumn(
                name: "role_id",
                table: "permission");

            migrationBuilder.RenameTable(
                name: "role",
                newName: "roles");

            migrationBuilder.RenameTable(
                name: "permission",
                newName: "permissions");

            migrationBuilder.AddPrimaryKey(
                name: "pk_roles",
                table: "roles",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_permissions",
                table: "permissions",
                column: "id");

            migrationBuilder.CreateTable(
                name: "role_permissions",
                columns: table => new
                {
                    role_id = table.Column<int>(type: "integer", nullable: false),
                    permission_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_role_permissions", x => new { x.role_id, x.permission_id });
                    table.ForeignKey(
                        name: "fk_role_permissions_permissions_permission_id",
                        column: x => x.permission_id,
                        principalTable: "permissions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_role_permissions_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "permissions",
                columns: new[] { "id", "name" },
                values: new object[] { 1, "users:read" });

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "id", "name" },
                values: new object[] { 1, "Registered" });

            migrationBuilder.InsertData(
                table: "role_permissions",
                columns: new[] { "permission_id", "role_id" },
                values: new object[] { 1, 1 });

            migrationBuilder.CreateIndex(
                name: "ix_role_permissions_permission_id",
                table: "role_permissions",
                column: "permission_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "role_permissions");

            migrationBuilder.DropPrimaryKey(
                name: "pk_roles",
                table: "roles");

            migrationBuilder.DropPrimaryKey(
                name: "pk_permissions",
                table: "permissions");

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.RenameTable(
                name: "roles",
                newName: "role");

            migrationBuilder.RenameTable(
                name: "permissions",
                newName: "permission");

            migrationBuilder.AddColumn<int>(
                name: "role_id",
                table: "permission",
                type: "integer",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "pk_role",
                table: "role",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_permission",
                table: "permission",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "ix_permission_role_id",
                table: "permission",
                column: "role_id");

            migrationBuilder.AddForeignKey(
                name: "fk_permission_role_role_id",
                table: "permission",
                column: "role_id",
                principalTable: "role",
                principalColumn: "id");
        }
    }
}
