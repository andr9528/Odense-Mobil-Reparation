using Repair.Abstractions.Entity.Searchable;
using Repair.Abstractions.Persistence;

namespace Repair.Abstractions.Entity.Model;

public interface ICustomer : ISearchableCustomer, IEntity
{
    ICollection<IOrder> Orders { get; set; }
}
