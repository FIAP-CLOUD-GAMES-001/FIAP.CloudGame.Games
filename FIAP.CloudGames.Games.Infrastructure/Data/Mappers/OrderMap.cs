using FIAP.CloudGames.Games.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FIAP.CloudGames.Games.Infrastructure.Data.Mappers;
public class OrderMap : IEntityTypeConfiguration<OrderEntity>
{
    public void Configure(EntityTypeBuilder<OrderEntity> builder)
    {
        builder.ToTable("Orders").HasKey(o => o.Id);
        builder.Property(o => o.Id).ValueGeneratedOnAdd().IsRequired();
        builder.Property(o => o.Status).IsRequired();
        builder.Property(o => o.UserId).IsRequired();
        builder.Property(o => o.CreatedAt).IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP");
        
        builder.HasMany(o => o.OrderGames)
            .WithOne(og => og.Order)
            .HasForeignKey(og => og.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

