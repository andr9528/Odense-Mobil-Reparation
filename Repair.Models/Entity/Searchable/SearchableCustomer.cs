using Repair.Abstractions.Entity.Searchable;

namespace Repair.Models.Entity.Searchable;

public class SearchableCustomer : ISearchableCustomer
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;
}
