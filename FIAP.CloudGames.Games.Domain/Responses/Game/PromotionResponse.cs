namespace FIAP.CloudGames.Games.Domain.Responses.Game;
public record PromotionResponse(int Id, string Title, decimal DiscountPercentage, DateTime StartDate, DateTime EndDate, int GameId);




