using Repair.Abstractions.Entity.Searchable;

namespace Repair.Models.Entity.Searchable;

public class SearchableOrder : ISearchableOrder
{
    public int Id { get; set; }

    public string HandInWhat { get; set; } = string.Empty;

    public string RepairWhat { get; set; } = string.Empty;

    public int CustomerId { get; set; }
}
