using FIAP.CloudGames.Games.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FIAP.CloudGames.Games.Infrastructure.Data;
public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<GameEntity> Games { get; set; } = null!;
    public DbSet<PromotionEntity> Promotions { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);
    }
}




