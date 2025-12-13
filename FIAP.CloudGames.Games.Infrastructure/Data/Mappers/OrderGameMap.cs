using FIAP.CloudGames.Games.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FIAP.CloudGames.Games.Infrastructure.Data.Mappers;
public class OrderGameMap : IEntityTypeConfiguration<OrderGameEntity>
{
    public void Configure(EntityTypeBuilder<OrderGameEntity> builder)
    {
        builder.ToTable("OrderGames").HasKey(og => new { og.OrderId, og.GameId });
        
        builder.Property(og => og.OrderId).IsRequired();
        builder.Property(og => og.GameId).IsRequired();
        
        builder.HasOne(og => og.Order)
            .WithMany(o => o.OrderGames)
            .HasForeignKey(og => og.OrderId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
        
        builder.HasOne(og => og.Game)
            .WithMany()
            .HasForeignKey(og => og.GameId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
    }
}


