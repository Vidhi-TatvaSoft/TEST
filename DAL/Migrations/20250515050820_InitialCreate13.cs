using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate13 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "status",
                table: "Jobs",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "isDelete",
                table: "Jobs",
                newName: "SsDelete");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "Jobs",
                newName: "CreatedAt");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "JobApplications",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "JobApplications",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "JobApplications");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "JobApplications");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Jobs",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "SsDelete",
                table: "Jobs",
                newName: "isDelete");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Jobs",
                newName: "CreatedDate");
        }
    }
}
