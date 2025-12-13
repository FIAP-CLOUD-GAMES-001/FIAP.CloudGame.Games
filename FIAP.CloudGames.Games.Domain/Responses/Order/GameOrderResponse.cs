using FIAP.CloudGames.Games.Domain.Enums;

namespace FIAP.CloudGames.Games.Domain.Responses.Order;

public record GameOrderResponse(
    int Id,
    string Title,
    string Description,
    decimal Price,
    EGameGenre Genre,
    DateTime ReleaseDate);

