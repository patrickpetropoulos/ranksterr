namespace Ranksterr.Domain.Items;

public interface IItemRepository
{
   Task<Item?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

   void Add(Item item);

}