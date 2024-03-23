using Ranksterr.Application.Abstractions.Messaging;
using Ranksterr.Domain.Item;

namespace Ranksterr.Application.Items.GetItem;

public sealed record GetItemQuery(Guid ItemId) : IQuery<Item>
{
    
}