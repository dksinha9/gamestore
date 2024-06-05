using GameStore.Api.Dtos;

namespace GameStore.Api.Endpoints;

public static class GamesEndpoints
{
    private static readonly List<GameDto> games = [
    new(1, "Street fighter II", "Fighting", 19.99M, new DateOnly(1992, 7, 15)),
    new GameDto(2, "The Legend of Zelda: Ocarina of Time", "Action-Adventure", 39.99M, new DateOnly(1998, 11, 21)),
    new GameDto(3, "Super Mario 64", "Platform", 29.99M, new DateOnly(1996, 6, 23)),
    new GameDto(4, "Final Fantasy VII", "RPG", 49.99M, new DateOnly(1997, 1, 31)),
    new GameDto(5, "Half-Life 2", "FPS", 19.99M, new DateOnly(2004, 11, 16)),
    new GameDto(6, "The Witcher 3: Wild Hunt", "RPG", 59.99M, new DateOnly(2015, 5, 19)),
    new GameDto(7, "Red Dead Redemption 2", "Action-Adventure", 59.99M, new DateOnly(2018, 10, 26)),
    new GameDto(8, "Minecraft", "Sandbox", 26.95M, new DateOnly(2011, 11, 18)),
    new GameDto(9, "Overwatch", "FPS", 39.99M, new DateOnly(2016, 5, 24)),
    new GameDto(10, "The Elder Scrolls V: Skyrim", "RPG", 39.99M, new DateOnly(2011, 11, 11)),
    new GameDto(11, "Dark Souls", "Action RPG", 19.99M, new DateOnly(2011, 9, 22)),
    new GameDto(12, "Doom Eternal", "FPS", 59.99M, new DateOnly(2020, 3, 20)),
    new GameDto(13, "Fortnite", "Battle Royale", 0.00M, new DateOnly(2017, 7, 25)),
    new GameDto(14, "Cyberpunk 2077", "RPG", 59.99M, new DateOnly(2020, 12, 10))
    ];
    const string GetGameEndpointName = "GetGame";

    public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app)
    {

        var group = app.MapGroup("games").WithParameterValidation();


        group.MapGet("", () => games);

        group.MapGet("/{id}", (int id) =>
        {
            GameDto? game = games.Find(game => game.Id == id);

            return game is null ? Results.NotFound() : Results.Ok(game);
        }
        ).WithName(GetGameEndpointName);

        group.MapPost("", (CreateGameDto newGame) =>
        {
            if (string.IsNullOrEmpty(newGame.Name))
            {
                return Results.BadRequest();
            }
            GameDto game = new(
                games.Count + 1,
                newGame.Name,
                newGame.Genre,
                newGame.Price,
                newGame.ReleaseDate
            );
            games.Add(game);

            return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, game);
        });

        group.MapPut("/{id}", (int id, UpdateGameDto updatedGame) =>
        {
            var index = games.FindIndex(game => game.Id == id);
            Console.WriteLine("OOPS");
            Console.WriteLine(index);
            if (index != -1)
            {

                games[index] = new GameDto(id, updatedGame.Name,
                updatedGame.Genre,
                updatedGame.Price,
                updatedGame.ReleaseDate);
                return Results.NoContent();
            }
            else
            {
                return Results.NotFound();
            }

        });

        group.MapDelete("/{id}", (int id) =>
        {
            games.RemoveAll(game => game.Id == id);

            return Results.NoContent();
        });

        return group;
    }
}
