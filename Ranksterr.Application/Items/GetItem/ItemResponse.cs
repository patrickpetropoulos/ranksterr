using System.Security.AccessControl;
using System.Security.Principal;

namespace Ranksterr.Application.Items.GetItem;

public class ItemResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}