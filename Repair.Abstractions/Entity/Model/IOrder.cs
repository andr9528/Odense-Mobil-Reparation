using Repair.Abstractions.Entity.Searchable;
using Repair.Abstractions.Persistence;

namespace Repair.Abstractions.Entity.Model;

public interface IOrder : ISearchableOrder, IEntity
{
    DateTime HandInWhen { get; set; }

    DateTime? ReturnedWhen { get; set; }

    bool IsOrderComplete { get; set; }

    ICustomer Customer { get; set; }
}
