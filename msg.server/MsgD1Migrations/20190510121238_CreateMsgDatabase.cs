using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace msg.server.MsgD1Migrations
{
    public partial class CreateMsgDatabase : Migration
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
                name: "Member",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    DialogueID = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Member", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Member_Dialogues_DialogueID",
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
                    DialogueID = table.Column<Guid>(nullable: true),
                    Text = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Messages_Dialogues_DialogueID",
                        column: x => x.DialogueID,
                        principalTable: "Dialogues",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Member_DialogueID",
                table: "Member",
                column: "DialogueID");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_DialogueID",
                table: "Messages",
                column: "DialogueID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Member");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "Dialogues");
        }
    }
}
