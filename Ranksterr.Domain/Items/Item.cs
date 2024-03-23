using Ranksterr.Domain.Abstractions;

namespace Ranksterr.Domain.Items;

public class Item : Entity
{
    public Guid Id { get; set; }
    public ItemName Name { get; set; }
    public ItemDescription Description { get; set; }
}