using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IXM.API.Migrations
{
    /// <inheritdoc />
    public partial class First : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "USERID",
                table: "Users",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<string>(
                name: "DeviceInfo",
                table: "Users",
                type: "BLOB SUB_TYPE TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "BLOB SUB_TYPE TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "DeviceCode",
                table: "Users",
                type: "BLOB SUB_TYPE TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "BLOB SUB_TYPE TEXT");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "USERID",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DeviceInfo",
                table: "Users",
                type: "BLOB SUB_TYPE TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "BLOB SUB_TYPE TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DeviceCode",
                table: "Users",
                type: "BLOB SUB_TYPE TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "BLOB SUB_TYPE TEXT",
                oldNullable: true);
        }
    }
}
