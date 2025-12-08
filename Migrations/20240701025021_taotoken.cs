using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebDatMonAn.Migrations
{
    /// <inheritdoc />
    public partial class taotoken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ResetToken",
                table: "KhachHangs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ResetTokenExpiry",
                table: "KhachHangs",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResetToken",
                table: "KhachHangs");

            migrationBuilder.DropColumn(
                name: "ResetTokenExpiry",
                table: "KhachHangs");
        }
    }
}
