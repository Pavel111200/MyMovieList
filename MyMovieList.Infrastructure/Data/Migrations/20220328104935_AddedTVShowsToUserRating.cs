using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyMovieList.Infrastructure.Data.Migrations
{
    public partial class AddedTVShowsToUserRating : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TVShowId",
                table: "UserRatings",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_UserRatings_TVShowId",
                table: "UserRatings",
                column: "TVShowId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRatings_TVShows_TVShowId",
                table: "UserRatings",
                column: "TVShowId",
                principalTable: "TVShows",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserRatings_TVShows_TVShowId",
                table: "UserRatings");

            migrationBuilder.DropIndex(
                name: "IX_UserRatings_TVShowId",
                table: "UserRatings");

            migrationBuilder.DropColumn(
                name: "TVShowId",
                table: "UserRatings");
        }
    }
}
