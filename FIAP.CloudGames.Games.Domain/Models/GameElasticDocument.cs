namespace FIAP.CloudGames.Games.Domain.Models;

public class GameElasticDocument
{
    public string Id { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public string Genre { get; set; } = string.Empty;

    public DateTime ReleaseDate { get; set; }

    public DateTime CreatedAt { get; set; }
}