using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WpfEFProfile.Migrations
{
    /// <inheritdoc />
    public partial class scaffolded_loratable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropPrimaryKey(
            //    name: "PK_TblUsers",
            //    table: "TblUsers");

            //migrationBuilder.RenameTable(
            //    name: "TblUsers",
            //    newName: "TblUser");

            //migrationBuilder.AddPrimaryKey(
            //    name: "PK_TblUser",
            //    table: "TblUser",
            //    column: "UserId");

            migrationBuilder.CreateTable(
                name: "TblLora",
                columns: table => new
                {
                    LoraId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CorSupply = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LoraSerial = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReceiptRv = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Refference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblLora", x => x.LoraId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropTable(
            //    name: "TblLora");

            //migrationBuilder.DropPrimaryKey(
            //    name: "PK_TblUser",
            //    table: "TblUser");

            //migrationBuilder.RenameTable(
            //    name: "TblUser",
            //    newName: "TblUsers");

            //migrationBuilder.AddPrimaryKey(
            //    name: "PK_TblUsers",
            //    table: "TblUsers",
            //    column: "UserId");
        }
    }
}
