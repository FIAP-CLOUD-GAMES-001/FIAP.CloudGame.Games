using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIAP.CloudGames.Games.Domain.Models;

public class GameElasticMetrics
{
    public double Total { get; set; }
    public GameElasticMetricsPriceRange PriceRange { get; set; } = new();
    public Dictionary<string, long> GamesByGenre { get; set; } = new();
    public Dictionary<int, long> GamesByReleaseYear { get; set; } = new();
}

public class GameElasticMetricsPriceRange
{
    public decimal Min { get; set; }
    public decimal Max { get; set; }
    public decimal Avg { get; set; }
    public decimal Sum { get; set; }
}