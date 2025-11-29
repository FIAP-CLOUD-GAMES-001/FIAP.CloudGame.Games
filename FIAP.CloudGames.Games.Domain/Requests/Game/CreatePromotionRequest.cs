namespace FIAP.CloudGames.Games.Domain.Requests.Game;
public record CreatePromotionRequest(string Title, decimal DiscountPercentage, DateTime StartDate, DateTime EndDate, int GameId);




