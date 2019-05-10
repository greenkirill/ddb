using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace msg.server.MsgD1Migrations
{
    public partial class Required : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Dialogues_DialogueID",
                table: "Messages");

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "Messages",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "DialogueID",
                table: "Messages",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Dialogues_DialogueID",
                table: "Messages",
                column: "DialogueID",
                principalTable: "Dialogues",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Dialogues_DialogueID",
                table: "Messages");

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "Messages",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<Guid>(
                name: "DialogueID",
                table: "Messages",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Dialogues_DialogueID",
                table: "Messages",
                column: "DialogueID",
                principalTable: "Dialogues",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
