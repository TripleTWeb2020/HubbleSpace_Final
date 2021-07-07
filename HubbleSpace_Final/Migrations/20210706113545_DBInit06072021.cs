using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HubbleSpace_Final.Migrations
{
    public partial class DBInit06072021 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmailSubscription",
                columns: table => new
                {
                    ID_EmailSubscription = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date_Created = table.Column<DateTime>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    subscribed_Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailSubscription", x => x.ID_EmailSubscription);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmailSubscription");
        }
    }
}
