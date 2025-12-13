using FIAP.CloudGames.Games.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FIAP.CloudGames.Games.Infrastructure.Data;
public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<GameEntity> Games { get; set; } = null!;
    public DbSet<PromotionEntity> Promotions { get; set; } = null!;
    public DbSet<OrderEntity> Orders { get; set; } = null!;
    public DbSet<OrderGameEntity> OrderGames { get; set; } = null!;
    public DbSet<PaymentEntity> Payments { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);
    }
}




