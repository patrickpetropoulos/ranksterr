using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ranksterr.Domain.Items;

namespace Ranksterr.Infrastructure.Configurations;

internal sealed class ItemConfiguration : IEntityTypeConfiguration<Item>
{
    public void Configure(EntityTypeBuilder<Item> builder)
    {
        builder.ToTable("Items");

        builder.HasKey(item => item.Id);

        builder.Property(item => item.Name)
            .HasMaxLength(200)
            .HasConversion(name => name.Value, value => new ItemName(value));
        
        builder.Property(item => item.Description)
            .HasMaxLength(2000)
            .HasConversion(description => description.Value, value => new ItemDescription(value));
        
        

    }

}