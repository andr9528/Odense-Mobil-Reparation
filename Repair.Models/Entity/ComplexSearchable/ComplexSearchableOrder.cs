using Repair.Abstractions.Persistence;
using Repair.Models.Entity.Searchable;

namespace Repair.Models.Entity.ComplexSearchable;

public class ComplexSearchableOrder : IComplexSearchable<SearchableOrder>
{
    public SearchableOrder Searchable { get; set; } = new();

    public string? HandInWhat { get; set; }

    public string? RepairWhat { get; set; }
}

