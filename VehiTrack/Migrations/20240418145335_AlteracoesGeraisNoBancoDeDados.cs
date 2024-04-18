using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehiTrack.Migrations
{
    /// <inheritdoc />
    public partial class AlteracoesGeraisNoBancoDeDados : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_vehicles_user_id",
                table: "vehicles");

            migrationBuilder.RenameColumn(
                name: "total_price",
                table: "refueling_records",
                newName: "unity_price");

            migrationBuilder.RenameColumn(
                name: "price",
                table: "refueling_records",
                newName: "total_cost");

            migrationBuilder.RenameColumn(
                name: "odometer",
                table: "refueling_records",
                newName: "odometer_counter");

            migrationBuilder.RenameColumn(
                name: "amount",
                table: "refueling_records",
                newName: "quantity");

            migrationBuilder.CreateIndex(
                name: "IX_vehicles_user_id_name",
                table: "vehicles",
                columns: new[] { "user_id", "name" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_vehicles_user_id_name",
                table: "vehicles");

            migrationBuilder.RenameColumn(
                name: "unity_price",
                table: "refueling_records",
                newName: "total_price");

            migrationBuilder.RenameColumn(
                name: "total_cost",
                table: "refueling_records",
                newName: "price");

            migrationBuilder.RenameColumn(
                name: "quantity",
                table: "refueling_records",
                newName: "amount");

            migrationBuilder.RenameColumn(
                name: "odometer_counter",
                table: "refueling_records",
                newName: "odometer");

            migrationBuilder.CreateIndex(
                name: "IX_vehicles_user_id",
                table: "vehicles",
                column: "user_id");
        }
    }
}
