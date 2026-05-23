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

        if (!string.IsNullOrWhiteSpace(orderComplex.CustomerName))
        {
            query = ApplyCustomerNameQuery(query, orderComplex);
        }

        if (orderComplex.IsOrderComplete.HasValue)
        {
            query = query.Where(x => x.IsOrderComplete == orderComplex.IsOrderComplete);
        }

        if (orderComplex.HasBorrowedPhone.HasValue)
        {
            query = query.Where(x => x.HasBorrowedPhone == orderComplex.HasBorrowedPhone);
        }

        return query;
    }

    private static IQueryable<Order> ApplyCustomerNameQuery(
        IQueryable<Order> query, ComplexSearchableOrder orderComplex)
    {
        if (!orderComplex.UseFuzzy)
        {
            return query.Where(x => x.Customer.Name.ToLower() == orderComplex.CustomerName!.ToLower());
        }

        var keyword = $"%{orderComplex.CustomerName}%";
        return query.Where(x => EF.Functions.Like(x.Customer.Name, keyword));
    }

    protected override IEnumerable<Order> ApplyComplexNonDatabaseQueryArguments(
        IEnumerable<Order> entities,
        IComplexSearchable<SearchableOrder> complex)
    {
        return entities;
    }

    protected override IQueryable<Order> GetBaseQuery()
    {
        return context.Orders.AsQueryable().Include(x => x.Customer);
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
