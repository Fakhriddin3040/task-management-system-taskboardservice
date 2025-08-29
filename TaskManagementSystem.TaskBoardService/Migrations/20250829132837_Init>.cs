using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManagementSystem.TaskBoardService.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "task_boards",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    organization_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_by_id = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_by_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<long>(type: "bigint", nullable: false),
                    updated_at = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_task_boards", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "task_board_columns",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    order = table.Column<long>(type: "bigint", nullable: false),
                    board_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_by_id = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_by_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<long>(type: "bigint", nullable: false),
                    updated_at = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_task_board_columns", x => x.id);
                    table.ForeignKey(
                        name: "fk_task_board_columns_task_boards_board_id",
                        column: x => x.board_id,
                        principalTable: "task_boards",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_task_board_columns_board_id",
                table: "task_board_columns",
                column: "board_id");

            migrationBuilder.CreateIndex(
                name: "ix_task_board_columns_name_board_id",
                table: "task_board_columns",
                columns: new[] { "name", "board_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_task_board_columns_order_board_id",
                table: "task_board_columns",
                columns: new[] { "order", "board_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_task_boards_organization_id",
                table: "task_boards",
                column: "organization_id");

            migrationBuilder.CreateIndex(
                name: "ix_task_boards_organization_id_name",
                table: "task_boards",
                columns: new[] { "organization_id", "name" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "task_board_columns");

            migrationBuilder.DropTable(
                name: "task_boards");
        }
    }
}
