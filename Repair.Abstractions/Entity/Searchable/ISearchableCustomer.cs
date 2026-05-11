using Repair.Abstractions.Persistence;

namespace Repair.Abstractions.Entity.Searchable;

public interface ISearchableCustomer : ISearchable
{
    string Name { get; set; }

    string Phone { get; set; }

    string Email { get; set; }
}
