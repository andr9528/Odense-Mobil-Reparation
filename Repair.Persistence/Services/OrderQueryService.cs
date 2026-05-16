using Microsoft.EntityFrameworkCore;
using Repair.Abstractions.Persistence;
using Repair.Models.Entity.ComplexSearchable;
using Repair.Models.Entity.Model;
using Repair.Models.Entity.Searchable;
using Repair.Persistence.Core;

namespace Repair.Persistence.Services;

public class OrderQueryService : BaseEntityQueryService<RepairDatabaseContext, Order, SearchableOrder>
{
    public OrderQueryService(RepairDatabaseContext context) : base(context)
    {
    }

    protected override IQueryable<Order> AddComplexQueryArguments(
        IQueryable<Order> query, IComplexSearchable<SearchableOrder> complex)
    {
        if (complex is not ComplexSearchableOrder orderComplex)
        {
            return query;
        }

        if (!string.IsNullOrWhiteSpace(orderComplex.HandInWhat))
        {
            var keyword = $"%{orderComplex.HandInWhat}%";

            query = query.Where(x => EF.Functions.Like(x.HandInWhat, keyword));
        }

        if (!string.IsNullOrWhiteSpace(orderComplex.RepairWhat))
        {
            var keyword = $"%{orderComplex.RepairWhat}%";

            query = query.Where(x => EF.Functions.Like(x.RepairWhat, keyword));
        }

        return query;
    }

    protected override IEnumerable<Order> ApplyComplexNonDatabaseQueryArguments(
        IEnumerable<Order> entities,
        IComplexSearchable<SearchableOrder> complex)
    {
        return entities;
    }

    protected override IQueryable<Order> GetBaseQuery()
    {
        return context.Orders.AsQueryable();
    }

    protected override IQueryable<Order> AddQueryArguments(
        SearchableOrder searchable,
        IQueryable<Order> query)
    {
        if (!string.IsNullOrWhiteSpace(searchable.HandInWhat))
            query = query.Where(x => x.HandInWhat.ToLower() == searchable.HandInWhat.ToLower());

        if (!string.IsNullOrWhiteSpace(searchable.RepairWhat))
            query = query.Where(x => x.RepairWhat.ToLower() == searchable.RepairWhat.ToLower());

        if (searchable.CustomerId != 0)
            query = query.Where(x => x.CustomerId == searchable.CustomerId);

        return query;
    }
}
