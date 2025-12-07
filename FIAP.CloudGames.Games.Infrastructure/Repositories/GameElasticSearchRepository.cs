using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Aggregations;
using Elastic.Clients.Elasticsearch.QueryDsl;
using FIAP.CloudGames.Games.Domain.Enums;
using FIAP.CloudGames.Games.Domain.Interfaces.Repositories;
using FIAP.CloudGames.Games.Domain.Models;

namespace FIAP.CloudGames.Games.Infrastructure.Repositories;

public class GameElasticSearchRepository(ElasticsearchClient client) : IGameElasticSearchRepository
{
    private readonly IndexName _index = "games";

    public async Task<GameElasticDocument?> GetByIdAsync(int id)
    {
        var response = await client.GetAsync<GameElasticDocument>(id, idx => idx.Index(_index));

        return response.Found ? response.Source : null;
    }

    public async Task IndexAsync(GameElasticDocument game)
    {
        var response = await client.IndexAsync(game, _index);

        if (!response.IsValidResponse)
            throw new Exception($"Failed to insert game in ElasticSearch: {response.ElasticsearchServerError?.Error.Reason}");
    }

    public async Task UpdateAsync(GameElasticDocument game)
    {
        var response = await client.UpdateAsync<GameElasticDocument, GameElasticDocument>(game.Id, upd => upd
            .Index(_index)
            .Doc(game)
        );

        if (!response.IsValidResponse)
            throw new Exception($"Failed to update game in ElasticSearch: {response.ElasticsearchServerError?.Error.Reason}");
    }

    public async Task DeleteAsync(int id)
    {
        var response = await client.DeleteAsync(index: _index, id);

        if (!response.IsValidResponse)
            throw new Exception($"Failed to remove game in ElasticSearch: {response.ElasticsearchServerError?.Error.Reason}");
    }

    public async Task<List<GameElasticDocument>> GetRecommendationsAsync(
        int gameId,
        EGameGenre genre,
        string description,
        int size = 10)
    {
        var genreValue = genre.ToString();
        var response = await client.SearchAsync<GameElasticDocument>(s => s
            .Index(_index)
            .Size(size)
            .Query(q => q
                .Bool(b => b
                    .Must(
                        // Filtrar games do mesmo gênero
                        m => m.Term(t => t
                            .Field(f => f.Genre.Suffix("keyword"))
                            .Value(genreValue)
                        ),
                        // Consulta as descrições dos outros games, onde busca uma similaridade de 20%
                        m => m.MoreLikeThis(mlt => mlt
                            .Fields("description")
                            .Like(new[]
                            {
                                new Like(description)
                            })
                            .MinTermFreq(1)
                            .MinDocFreq(1)
                            .MaxQueryTerms(10)
                            .MinimumShouldMatch("10%")
                        )
                    )
                    // Ignorar o game consultado
                    .MustNot(
                        mn => mn.Term(t => t
                            .Field(f => f.Id)
                            .Value(gameId)
                        )
                    )
                )
            )
        );

        if (!response.IsValidResponse)
        {
            return new List<GameElasticDocument>();
        }

        return response.Documents.ToList();
    }

    public async Task<GameElasticMetrics> GetMetricsAsync()
    {
        var response = await client.SearchAsync<GameElasticDocument>(s => s
            .Indices(_index)
            .Aggregations(aggregations => aggregations
                .Add("total_games", aggregation => aggregation.ValueCount(vc => vc.Field(f => f.Id.Suffix("keyword"))))
                .Add("price_stats", aggregation => aggregation.Stats(vc => vc.Field(f => f.Price)))
                .Add("genres", aggregation => aggregation.Terms(vc => vc.Field(f => f.Genre.Suffix("keyword")).Size(50)))
                .Add("release_years", aggregation => aggregation.DateHistogram(vc => vc.Field(f => f.ReleaseDate).CalendarInterval(CalendarInterval.Year)))
            )
            .Size(10)
        );

        var result = new GameElasticMetrics();

        var total = response.Aggregations!.GetValueCount("total_games");
        result.Total = total?.Value ?? 0;

        var priceStats = response.Aggregations!.GetStats("price_stats");
        if (priceStats != null)
        {
            result.PriceRange = new GameElasticMetricsPriceRange
            {
                Min = (decimal)(priceStats.Min ?? 0),
                Max = (decimal)(priceStats.Max ?? 0),
                Avg = (decimal)(priceStats.Avg ?? 0),
                Sum = (decimal)priceStats.Sum
            };
        }

        var gamesByGenre = response.Aggregations.GetStringTerms("genres");
        if (gamesByGenre != null)
        {
            foreach (var bucket in gamesByGenre.Buckets)
            {
                result.GamesByGenre[bucket.Key.ToString()] = bucket.DocCount;
            }
        }

        var gamesByYear = response.Aggregations.GetDateHistogram("release_years");
        if (gamesByYear != null)
        {
            foreach (var bucket in gamesByYear.Buckets)
            {
                if (int.TryParse(bucket.KeyAsString?.Substring(0, 4), out var year))
                {
                    result.GamesByReleaseYear[year] = bucket.DocCount;
                }
            }
        }

        return result;
    }
}