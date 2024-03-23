using Ranksterr.Application.Abstractions.Messaging;
using Ranksterr.Domain.Abstractions;
using Ranksterr.Domain.Item;

namespace Ranksterr.Application.Items.GetItem;

internal sealed class GetItemQueryHandler: IQueryHandler<GetItemQuery, Item>
{
    public async Task<Result<Item>> Handle(GetItemQuery request, CancellationToken cancellationToken)
    {
        var item = new Item()
        {
            Id = Guid.NewGuid(),
            Name = "Test Item"
        };
        var itemResult = Result.Success(item);
        return itemResult;
    }
}