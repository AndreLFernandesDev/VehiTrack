using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehiTrack.Migrations
{
    /// <inheritdoc />
    public partial class CreateColumnDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "date",
                table: "refueling_records",
                nullable: false
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder) { }
    }
}
