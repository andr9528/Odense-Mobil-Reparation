using Repair.Abstractions.Persistence;
using Repair.Models.Entity.Searchable;

namespace Repair.Models.Entity.ComplexSearchable;

public class ComplexSearchableCustomer : IComplexSearchable<SearchableCustomer>
{
    /// <inheritdoc />
    public SearchableCustomer Searchable { get; set; } = new();

    public string? Name { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
}
