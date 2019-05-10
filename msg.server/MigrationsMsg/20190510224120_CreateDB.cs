using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace msg.server.MigrationsMsg
{
    public partial class CreateDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Dialogues",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dialogues", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Members",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    MemberID = table.Column<Guid>(nullable: false),
                    DialogueID = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Members", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Members_Dialogues_DialogueID",
                        column: x => x.DialogueID,
                        principalTable: "Dialogues",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    SentAt = table.Column<DateTime>(nullable: false),
                    SentBy = table.Column<Guid>(nullable: false),
                    DialogueID = table.Column<Guid>(nullable: false),
                    Text = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Messages_Dialogues_DialogueID",
                        column: x => x.DialogueID,
                        principalTable: "Dialogues",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Members_DialogueID",
                table: "Members",
                column: "DialogueID");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_DialogueID",
                table: "Messages",
                column: "DialogueID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Members");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "Dialogues");
        }
    }
}
