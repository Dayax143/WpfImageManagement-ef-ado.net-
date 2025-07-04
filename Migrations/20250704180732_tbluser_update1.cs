using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WpfEFProfile.Migrations
{
    /// <inheritdoc />
    public partial class tbluser_update1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AddColumn<string>(
            //    name: "PasswordHash",
            //    table: "TblUsers",
            //    type: "nvarchar(max)",
            //    nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropColumn(
            //    name: "PasswordHash",
            //    table: "TblUsers");
        }
    }
}
