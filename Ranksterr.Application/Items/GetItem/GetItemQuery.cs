using Ranksterr.Application.Abstractions.Messaging;
using Ranksterr.Domain.Items;

namespace Ranksterr.Application.Items.GetItem;

public sealed record GetItemQuery(Guid ItemId) : IQuery<ItemResponse>
{
    
}