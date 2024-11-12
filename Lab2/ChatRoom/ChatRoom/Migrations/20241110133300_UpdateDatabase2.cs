using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatRoom.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDatabase2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatRoomUsers_ChatRooms_ChatRoomsChatRoomEntityId",
                table: "ChatRoomUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_ChatRoomUsers_Users_UserId",
                table: "ChatRoomUsers");

            migrationBuilder.DropIndex(
                name: "IX_ChatRoomUsers_UserId",
                table: "ChatRoomUsers");

            migrationBuilder.DropColumn(
                name: "ChatRoomsChatRoomEntityId",
                table: "ChatRoomUsers");

            migrationBuilder.CreateTable(
                name: "ChatRoomEntityUser",
                columns: table => new
                {
                    ChatRoomsChatRoomEntityId = table.Column<int>(type: "INTEGER", nullable: false),
                    UsersUserId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatRoomEntityUser", x => new { x.ChatRoomsChatRoomEntityId, x.UsersUserId });
                    table.ForeignKey(
                        name: "FK_ChatRoomEntityUser_ChatRooms_ChatRoomsChatRoomEntityId",
                        column: x => x.ChatRoomsChatRoomEntityId,
                        principalTable: "ChatRooms",
                        principalColumn: "ChatRoomEntityId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChatRoomEntityUser_Users_UsersUserId",
                        column: x => x.UsersUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChatRoomEntityUser_UsersUserId",
                table: "ChatRoomEntityUser",
                column: "UsersUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChatRoomEntityUser");

            migrationBuilder.AddColumn<int>(
                name: "ChatRoomsChatRoomEntityId",
                table: "ChatRoomUsers",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChatRoomUsers_UserId",
                table: "ChatRoomUsers",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatRoomUsers_ChatRooms_ChatRoomsChatRoomEntityId",
                table: "ChatRoomUsers",
                column: "ChatRoomsChatRoomEntityId",
                principalTable: "ChatRooms",
                principalColumn: "ChatRoomEntityId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChatRoomUsers_Users_UserId",
                table: "ChatRoomUsers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
