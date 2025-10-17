using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GUI_Programmering_WebApi.Migrations
{
    /// <inheritdoc />
    public partial class AddImageTableAndProductRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageLink",
                table: "Products",
                newName: "ImageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageId",
                table: "Products",
                newName: "ImageLink");
        }
    }
}
