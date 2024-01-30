using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SMTS.Migrations
{
    public partial class JobOrderInit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JobOrders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobOrderNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobOrders", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "82af61cb-5895-4b04-82fd-352437d7b8a2", "AQAAAAEAACcQAAAAEKFo2UWA94wmEFXxCFoj5OFu1r65Fra4x97lrbJVpWE8nucJEXcnyyGfugdQf6jKzQ==", "efa894b3-ae98-477f-b405-6683fdb6ee7c" });

            migrationBuilder.InsertData(
                table: "JobOrders",
                columns: new[] { "Id", "JobOrderNo", "Status" },
                values: new object[,]
                {
                    { 1, "JO001", "Pending" },
                    { 2, "JO002", "Completed" },
                    { 3, "JO003", "In Progress" },
                    { 4, "JO004", "Canceled" },
                    { 5, "JO005", "Pending" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JobOrders");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "35eb4bb9-02e2-475f-aca8-318a2ea9fc3e", "AQAAAAEAACcQAAAAEOdV41AeEoNl5EsMyZF9VJUa5mslPGshOPoY0KA34G+x+3H7FeGXY/UTwvCPSe+4Mw==", "fda34c96-20b5-410d-95ad-a30bfa156242" });
        }
    }
}
