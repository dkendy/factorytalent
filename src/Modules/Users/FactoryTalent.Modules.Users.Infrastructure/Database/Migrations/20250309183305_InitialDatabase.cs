using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FactoryTalent.Modules.Users.Infrastructure.Database.Migrations;

/// <inheritdoc />
public partial class InitialDatabase : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.EnsureSchema(
            name: "users");

        migrationBuilder.CreateTable(
            name: "permissions",
            schema: "users",
            columns: table => new
            {
                code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_permissions", x => x.code);
            });

        migrationBuilder.CreateTable(
            name: "roles",
            schema: "users",
            columns: table => new
            {
                name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                level = table.Column<int>(type: "integer", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_roles", x => x.name);
            });

        migrationBuilder.CreateTable(
            name: "users",
            schema: "users",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                email = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                first_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                last_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                cpf = table.Column<string>(type: "character varying(11)", maxLength: 11, nullable: false),
                address = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                birth_day = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                superior_id = table.Column<Guid>(type: "uuid", nullable: true),
                identity_id = table.Column<string>(type: "text", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_users", x => x.id);
                table.ForeignKey(
                    name: "fk_users_users_superior_id",
                    column: x => x.superior_id,
                    principalSchema: "users",
                    principalTable: "users",
                    principalColumn: "id",
                    onDelete: ReferentialAction.SetNull);
            });

        migrationBuilder.CreateTable(
            name: "role_permissions",
            schema: "users",
            columns: table => new
            {
                permission_code = table.Column<string>(type: "character varying(100)", nullable: false),
                role_name = table.Column<string>(type: "character varying(50)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_role_permissions", x => new { x.permission_code, x.role_name });
                table.ForeignKey(
                    name: "fk_role_permissions_permissions_permission_code",
                    column: x => x.permission_code,
                    principalSchema: "users",
                    principalTable: "permissions",
                    principalColumn: "code",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "fk_role_permissions_roles_role_name",
                    column: x => x.role_name,
                    principalSchema: "users",
                    principalTable: "roles",
                    principalColumn: "name",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "contacts",
            schema: "users",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                phone_number = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                user_id = table.Column<Guid>(type: "uuid", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_contacts", x => x.id);
                table.ForeignKey(
                    name: "fk_contacts_users_user_id",
                    column: x => x.user_id,
                    principalSchema: "users",
                    principalTable: "users",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "user_roles",
            schema: "users",
            columns: table => new
            {
                role_name = table.Column<string>(type: "character varying(50)", nullable: false),
                user_id = table.Column<Guid>(type: "uuid", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_user_roles", x => new { x.role_name, x.user_id });
                table.ForeignKey(
                    name: "fk_user_roles_roles_roles_name",
                    column: x => x.role_name,
                    principalSchema: "users",
                    principalTable: "roles",
                    principalColumn: "name",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "fk_user_roles_users_user_id",
                    column: x => x.user_id,
                    principalSchema: "users",
                    principalTable: "users",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.InsertData(
            schema: "users",
            table: "permissions",
            column: "code",
            values: new object[]
            {
                "users:add",
                "users:delete",
                "users:read",
                "users:update"
            });

        migrationBuilder.InsertData(
            schema: "users",
            table: "roles",
            columns: ["name", "level" ],
            values: new object[,]
            {
                { "Administrator", 4 },
                { "Employee", 2 },
                { "Intern", 1 },
                { "Manager", 3 }
            });

        migrationBuilder.InsertData(
            schema: "users",
            table: "role_permissions",
            columns: [ "permission_code", "role_name"],
            values: new object[,]
            {
                { "users:add", "Administrator" },
                { "users:add", "Manager" },
                { "users:delete", "Administrator" },
                { "users:delete", "Manager" },
                { "users:read", "Administrator" },
                { "users:read", "Employee" },
                { "users:read", "Intern" },
                { "users:read", "Manager" },
                { "users:update", "Administrator" },
                { "users:update", "Employee" },
                { "users:update", "Intern" },
                { "users:update", "Manager" }
            });

        migrationBuilder.CreateIndex(
            name: "ix_contacts_user_id",
            schema: "users",
            table: "contacts",
            column: "user_id");

        migrationBuilder.CreateIndex(
            name: "ix_role_permissions_role_name",
            schema: "users",
            table: "role_permissions",
            column: "role_name");

        migrationBuilder.CreateIndex(
            name: "ix_user_roles_user_id",
            schema: "users",
            table: "user_roles",
            column: "user_id");

        migrationBuilder.CreateIndex(
            name: "ix_users_cpf",
            schema: "users",
            table: "users",
            column: "cpf",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "ix_users_email",
            schema: "users",
            table: "users",
            column: "email",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "ix_users_identity_id",
            schema: "users",
            table: "users",
            column: "identity_id");

        migrationBuilder.CreateIndex(
            name: "ix_users_superior_id",
            schema: "users",
            table: "users",
            column: "superior_id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "contacts",
            schema: "users");

        migrationBuilder.DropTable(
            name: "role_permissions",
            schema: "users");

        migrationBuilder.DropTable(
            name: "user_roles",
            schema: "users");

        migrationBuilder.DropTable(
            name: "permissions",
            schema: "users");

        migrationBuilder.DropTable(
            name: "roles",
            schema: "users");

        migrationBuilder.DropTable(
            name: "users",
            schema: "users");
    }
}
