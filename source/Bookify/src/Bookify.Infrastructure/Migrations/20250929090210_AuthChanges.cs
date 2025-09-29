using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Bookify.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AuthChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "identity_id",
                table: "users",
                type: "text",
                nullable: false,
                defaultValue: ""
            );

            migrationBuilder.CreateTable(
                name: "role",
                columns: table => new
                {
                    id = table
                        .Column<int>(type: "integer", nullable: false)
                        .Annotation(
                            "Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                        ),
                    name = table.Column<string>(type: "text", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_role", x => x.id);
                }
            );

            migrationBuilder.CreateTable(
                name: "permission",
                columns: table => new
                {
                    id = table
                        .Column<int>(type: "integer", nullable: false)
                        .Annotation(
                            "Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                        ),
                    name = table.Column<string>(type: "text", nullable: false),
                    role_id = table.Column<int>(type: "integer", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_permission", x => x.id);
                    table.ForeignKey(
                        name: "fk_permission_role_role_id",
                        column: x => x.role_id,
                        principalTable: "role",
                        principalColumn: "id"
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "role_user",
                columns: table => new
                {
                    roles_id = table.Column<int>(type: "integer", nullable: false),
                    users_id = table.Column<Guid>(type: "uuid", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_role_user", x => new { x.roles_id, x.users_id });
                    table.ForeignKey(
                        name: "fk_role_user_role_roles_id",
                        column: x => x.roles_id,
                        principalTable: "role",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade
                    );
                    table.ForeignKey(
                        name: "fk_role_user_user_users_id",
                        column: x => x.users_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateIndex(
                name: "ix_permission_role_id",
                table: "permission",
                column: "role_id"
            );

            migrationBuilder.CreateIndex(
                name: "ix_role_user_users_id",
                table: "role_user",
                column: "users_id"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "permission");

            migrationBuilder.DropTable(name: "role_user");

            migrationBuilder.DropTable(name: "role");

            migrationBuilder.DropColumn(name: "identity_id", table: "users");
        }
    }
}
