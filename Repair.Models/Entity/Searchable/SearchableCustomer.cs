using Repair.Abstractions.Entity.Searchable;

namespace Repair.Models.Entity.Searchable;

public class SearchableCustomer : ISearchableCustomer
{
    /// <inheritdoc />
    public int Id { get; set; }

    /// <inheritdoc />
    public string Name { get; set; } = string.Empty;

    /// <inheritdoc />
    public string Phone { get; set; } = string.Empty;

    /// <inheritdoc />
    public string Email { get; set; } = string.Empty;
}
