using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoolBro.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MessageResponse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Response",
                table: "Messages",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Response",
                table: "Messages");
        }
    }
}
