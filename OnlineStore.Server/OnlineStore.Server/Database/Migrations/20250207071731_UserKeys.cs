using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace OnlineStore.Server.Database.Migrations
{
    /// <inheritdoc />
    public partial class UserKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "1to1_customer_to_user_fk",
                table: "users_table");

            migrationBuilder.DropPrimaryKey(
                name: "id_user_pk",
                table: "users_table");

            migrationBuilder.AlterColumn<Guid>(
                name: "customer_id",
                table: "users_table",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<int>(
                name: "id",
                table: "users_table",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "id_user_pk",
                table: "users_table",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "IX_users_table_customer_id",
                table: "users_table",
                column: "customer_id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "1to1_customer_to_user_fk",
                table: "users_table",
                column: "customer_id",
                principalTable: "customers",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "1to1_customer_to_user_fk",
                table: "users_table");

            migrationBuilder.DropPrimaryKey(
                name: "id_user_pk",
                table: "users_table");

            migrationBuilder.DropIndex(
                name: "IX_users_table_customer_id",
                table: "users_table");

            migrationBuilder.DropColumn(
                name: "id",
                table: "users_table");

            migrationBuilder.AlterColumn<Guid>(
                name: "customer_id",
                table: "users_table",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "id_user_pk",
                table: "users_table",
                column: "customer_id");

            migrationBuilder.AddForeignKey(
                name: "1to1_customer_to_user_fk",
                table: "users_table",
                column: "customer_id",
                principalTable: "customers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
