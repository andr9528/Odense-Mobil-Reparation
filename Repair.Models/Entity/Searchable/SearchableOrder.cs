using Repair.Abstractions.Entity.Searchable;

namespace Repair.Models.Entity.Searchable;

public class SearchableOrder : ISearchableOrder
{
    /// <inheritdoc />
    public int Id { get; set; }

    /// <inheritdoc />
    public string HandInWhat { get; set; } = string.Empty;

    /// <inheritdoc />
    public string RepairWhat { get; set; } = string.Empty;

    /// <inheritdoc />
    public int CustomerId { get; set; }

    /// <inheritdoc />
    public string? BorrowedPhone { get; set; }
}
