using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyMovieList.Infrastructure.Data.Migrations
{
    public partial class TVShowColumnNameChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Episode",
                table: "TVShows",
                newName: "NumberOfEpisodes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NumberOfEpisodes",
                table: "TVShows",
                newName: "Episode");
        }
    }
}
