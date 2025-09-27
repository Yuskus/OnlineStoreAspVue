using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace OnlineStore.Server.Database.Migrations
{
    /// <inheritdoc />
    public partial class UserIdChanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "1tomany_order_to_order_elements_fk",
                table: "order_elements");

            migrationBuilder.DropForeignKey(
                name: "manyto1_order_elements_to_item_fk",
                table: "order_elements");

            migrationBuilder.AlterColumn<Guid>(
                name: "id",
                table: "users_table",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddForeignKey(
                name: "1tomany_item_to_order_elements_fk",
                table: "order_elements",
                column: "item_id",
                principalTable: "items",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "manyto1_order_elements_to_order_fk",
                table: "order_elements",
                column: "order_id",
                principalTable: "orders",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "1tomany_item_to_order_elements_fk",
                table: "order_elements");

            migrationBuilder.DropForeignKey(
                name: "manyto1_order_elements_to_order_fk",
                table: "order_elements");

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "users_table",
                type: "integer",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddForeignKey(
                name: "1tomany_order_to_order_elements_fk",
                table: "order_elements",
                column: "order_id",
                principalTable: "orders",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "manyto1_order_elements_to_item_fk",
                table: "order_elements",
                column: "item_id",
                principalTable: "items",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
