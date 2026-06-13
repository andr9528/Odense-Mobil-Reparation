using Repair.Abstractions.Persistence;
using Repair.Models.Entity.Searchable;

namespace Repair.Models.Entity.ComplexSearchable;

public class ComplexSearchableOrder : IComplexSearchable<SearchableOrder>
{
    public SearchableOrder Searchable { get; set; } = new();

    public string? HandInWhat { get; set; }
    public string? RepairWhat { get; set; }
    public string? CustomerName { get; set; }
    public string? BorrowedPhone { get; set; }

    public bool UseFuzzy { get; set; }

    public bool? IsOrderComplete { get; set; }

    public DateTime? HandInFrom { get; set; }
    public DateTime? HandInTo { get; set; }
    public DateTime? ReturnedFrom { get; set; }
    public DateTime? ReturnedTo { get; set; }
}

