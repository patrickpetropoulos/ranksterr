using Dapper;
using Ranksterr.Application.Abstractions.Data;
using Ranksterr.Application.Abstractions.Messaging;
using Ranksterr.Domain.Abstractions;
using Ranksterr.Domain.Items;

namespace Ranksterr.Application.Items.GetItem;

internal sealed class GetItemQueryHandler: IQueryHandler<GetItemQuery, ItemResponse>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;
    private readonly IItemRepository _itemRepository;

    public GetItemQueryHandler(ISqlConnectionFactory sqlConnectionFactory, IItemRepository itemRepository)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
        _itemRepository = itemRepository;
    }

    public async Task<Result<ItemResponse>> Handle(GetItemQuery request, CancellationToken cancellationToken)
    {
        var itemFromRepo = await _itemRepository.GetByIdAsync(Guid.Parse("f28c0109-2a99-42f8-a27c-7bbf4036ad0f"), cancellationToken);
        
        
        using var connection = _sqlConnectionFactory.CreateConnection();

        const string sql = """
                            SELECT
                            Id,
                            Name,
                            Description
                            from items
                            where id = @ItemId
                           """;
        var item = await connection.QueryFirstOrDefaultAsync<ItemResponse>(
            sql,
            new
            {
                request.ItemId
            });
        if (item is null)
        {
            return Result.Failure<ItemResponse>(Error.None);
        }

        return item;
    }
}