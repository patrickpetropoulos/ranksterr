using Ranksterr.Domain.Abstractions;

namespace Ranksterr.Domain.Item;

public class Item : Entity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    
}