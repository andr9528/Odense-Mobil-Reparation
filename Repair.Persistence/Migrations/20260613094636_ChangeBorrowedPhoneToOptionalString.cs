using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repair.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChangeBorrowedPhoneToOptionalString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasBorrowedPhone",
                table: "Orders");

            migrationBuilder.AddColumn<string>(
                name: "BorrowedPhone",
                table: "Orders",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BorrowedPhone",
                table: "Orders");

            migrationBuilder.AddColumn<bool>(
                name: "HasBorrowedPhone",
                table: "Orders",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }
    }
}
