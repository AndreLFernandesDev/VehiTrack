using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehiTrack.Migrations
{
    /// <inheritdoc />
    public partial class CreatingAutomaticColumnCalculationTotalCost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "total_cost",
                table: "refueling_records",
                type: "double precision",
                nullable: false,
                computedColumnSql: "quantity * unity_price",
                stored: true,
                oldClrType: typeof(double),
                oldType: "double precision");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "total_cost",
                table: "refueling_records",
                type: "double precision",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldComputedColumnSql: "quantity * unity_price");
        }
    }
}
