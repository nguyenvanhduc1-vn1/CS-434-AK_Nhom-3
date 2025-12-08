using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebDatMonAn.Migrations
{
    /// <inheritdoc />
    public partial class token : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ResetToken",
                table: "Shippers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ResetTokenExpiry",
                table: "Shippers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResetToken",
                table: "NhanViens",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ResetTokenExpiry",
                table: "NhanViens",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResetToken",
                table: "Shippers");

            migrationBuilder.DropColumn(
                name: "ResetTokenExpiry",
                table: "Shippers");

            migrationBuilder.DropColumn(
                name: "ResetToken",
                table: "NhanViens");

            migrationBuilder.DropColumn(
                name: "ResetTokenExpiry",
                table: "NhanViens");
        }
    }
}
