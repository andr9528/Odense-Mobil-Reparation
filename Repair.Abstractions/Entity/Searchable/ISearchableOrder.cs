using Repair.Abstractions.Persistence;

namespace Repair.Abstractions.Entity.Searchable;

public interface ISearchableOrder : ISearchable
{
    string HandInWhat { get; set; }

    string RepairWhat { get; set; }

    int CustomerId { get; set; }
}
