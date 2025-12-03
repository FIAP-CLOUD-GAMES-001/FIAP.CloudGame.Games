using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using FIAP.CloudGames.Games.Domain.Entities;
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
                            .MaxQueryTerms(20)
                            .MinimumShouldMatch("20%")
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
}