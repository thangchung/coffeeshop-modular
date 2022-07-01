using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoffeeShop.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitCoffeeShopDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "barista");

            migrationBuilder.EnsureSchema(
                name: "kitchen");

            migrationBuilder.EnsureSchema(
                name: "order");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:uuid-ossp", ",,");

            migrationBuilder.CreateTable(
                name: "barista_orders",
                schema: "barista",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    item_type = table.Column<int>(type: "integer", nullable: false),
                    time_in = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    time_up = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_barista_orders", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "kitchen_orders",
                schema: "kitchen",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    order_id = table.Column<Guid>(type: "uuid", nullable: false),
                    item_type = table.Column<int>(type: "integer", nullable: false),
                    time_in = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    time_up = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_kitchen_orders", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "orders",
                schema: "order",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    order_source = table.Column<int>(type: "integer", nullable: false),
                    loyalty_member_id = table.Column<Guid>(type: "uuid", nullable: false),
                    order_status = table.Column<int>(type: "integer", nullable: false),
                    location = table.Column<int>(type: "integer", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_orders", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "line_items",
                schema: "order",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    order_id = table.Column<Guid>(type: "uuid", nullable: false),
                    item_type = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    price = table.Column<decimal>(type: "numeric", nullable: false),
                    item_status = table.Column<int>(type: "integer", nullable: false),
                    is_barista_order = table.Column<bool>(type: "boolean", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_line_items", x => x.id);
                    table.ForeignKey(
                        name: "fk_line_items_orders_order_temp_id",
                        column: x => x.order_id,
                        principalSchema: "order",
                        principalTable: "orders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_barista_orders_id",
                schema: "barista",
                table: "barista_orders",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_kitchen_orders_id",
                schema: "kitchen",
                table: "kitchen_orders",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_line_items_id",
                schema: "order",
                table: "line_items",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_line_items_order_id",
                schema: "order",
                table: "line_items",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "ix_orders_id",
                schema: "order",
                table: "orders",
                column: "id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "barista_orders",
                schema: "barista");

            migrationBuilder.DropTable(
                name: "kitchen_orders",
                schema: "kitchen");

            migrationBuilder.DropTable(
                name: "line_items",
                schema: "order");

            migrationBuilder.DropTable(
                name: "orders",
                schema: "order");
        }
    }
}
