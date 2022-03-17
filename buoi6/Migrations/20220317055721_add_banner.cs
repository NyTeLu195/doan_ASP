﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace buoi6.Migrations
{
    public partial class add_banner : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Banner",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Img_banner = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title_banner = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Des = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banner", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Banner");
        }
    }
}
