using FIAP.CloudGames.Games.Domain.Enums;

namespace FIAP.CloudGames.Games.Domain.Responses.Game;
public record GameResponse(int Id, string Title, string Description, decimal Price, EGameGenre Genre, DateTime ReleaseDate);




