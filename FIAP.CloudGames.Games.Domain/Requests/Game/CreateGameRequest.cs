using FIAP.CloudGames.Games.Domain.Enums;

namespace FIAP.CloudGames.Games.Domain.Requests.Game;
public record CreateGameRequest(string Title, string Description, decimal Price, EGameGenre Genre, DateTime ReleaseDate);




