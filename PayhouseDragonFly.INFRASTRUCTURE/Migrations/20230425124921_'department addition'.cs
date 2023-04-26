using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PayhouseDragonFly.INFRASTRUCTURE.Migrations
{
    /// <inheritdoc />
    public partial class departmentaddition : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DepartmentDescription",
                table: "PayhouseDragonFlyUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DepartmentDescription",
                table: "PayhouseDragonFlyUsers");
        }
    }
}
