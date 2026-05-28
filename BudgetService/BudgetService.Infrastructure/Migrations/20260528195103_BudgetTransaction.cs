using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class BudgetTransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "TotalSpent",
                table: "Budgets",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalSpent",
                table: "Budgets");
        }
    }
}
