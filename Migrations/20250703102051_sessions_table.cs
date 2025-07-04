using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WpfEFProfile.Migrations
{
    /// <inheritdoc />
    public partial class sessions_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateTable(
            //    name: "sessions",
            //    columns: table => new
            //    {
            //        ses_id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        ses_user = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ses_time = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        ses_status = table.Column<string>(type: "nvarchar(max)", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_sessions", x => x.ses_id);
            //    });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropTable(
            //    name: "sessions");
        }
    }
}
