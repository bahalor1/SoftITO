using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KütüphaneUygulaması.Migrations
{
    /// <inheritdoc />
    public partial class KategoriIsimDuzeltme : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Kitaplar_Kategoiler_KategoriId",
                table: "Kitaplar");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Kategoiler",
                table: "Kategoiler");

            migrationBuilder.RenameTable(
                name: "Kategoiler",
                newName: "Kategoriler");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Kategoriler",
                table: "Kategoriler",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Kitaplar_Kategoriler_KategoriId",
                table: "Kitaplar",
                column: "KategoriId",
                principalTable: "Kategoriler",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Kitaplar_Kategoriler_KategoriId",
                table: "Kitaplar");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Kategoriler",
                table: "Kategoriler");

            migrationBuilder.RenameTable(
                name: "Kategoriler",
                newName: "Kategoiler");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Kategoiler",
                table: "Kategoiler",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Kitaplar_Kategoiler_KategoriId",
                table: "Kitaplar",
                column: "KategoriId",
                principalTable: "Kategoiler",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
