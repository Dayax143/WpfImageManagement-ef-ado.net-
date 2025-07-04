using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WpfEFProfile.Migrations
{
    /// <inheritdoc />
    public partial class tbluser_totestDB1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateTable(
            //    name: "TblUsers",
            //    columns: table => new
            //    {
            //        UserId = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        PassWord = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Usertype = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        RecoveryQuestion = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        RecoveryAnswer = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        UserStatus = table.Column<string>(type: "nvarchar(max)", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_TblUsers", x => x.UserId);
            //    });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropTable(
            //    name: "TblUsers");
        }
    }
}
