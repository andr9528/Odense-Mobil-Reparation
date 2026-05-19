using Microsoft.EntityFrameworkCore;
using Repair.Abstractions.Persistence;
using Repair.Models.Entity.ComplexSearchable;
using Repair.Models.Entity.Model;
using Repair.Models.Entity.Searchable;
using Repair.Persistence.Core;

namespace Repair.Persistence.Services;

public class CustomerQueryService : BaseEntityQueryService<RepairDatabaseContext, Customer, SearchableCustomer>
{
    public CustomerQueryService(RepairDatabaseContext context) : base(context)
    {
    }

    protected override IQueryable<Customer> AddComplexQueryArguments(
        IQueryable<Customer> query, IComplexSearchable<SearchableCustomer> complex)
    {
        if (complex is not ComplexSearchableCustomer customerComplex)
        {
            return query;
        }

        if (!string.IsNullOrWhiteSpace(customerComplex.Name))
        {
            var keyword = $"%{customerComplex.Name}%";

            query = query.Where(x => EF.Functions.Like(x.Name, keyword));
        }

        if (!string.IsNullOrWhiteSpace(customerComplex.Phone))
        {
            var keyword = $"%{customerComplex.Phone}%";

            query = query.Where(x => EF.Functions.Like(x.Phone, keyword));
        }

        if (!string.IsNullOrWhiteSpace(customerComplex.Email))
        {
            var keyword = $"%{customerComplex.Email}%";

            query = query.Where(x => EF.Functions.Like(x.Email, keyword));
        }

        return query;
    }

    protected override IEnumerable<Customer> ApplyComplexNonDatabaseQueryArguments(
        IEnumerable<Customer> entities,
        IComplexSearchable<SearchableCustomer> complex)
    {
        return entities;
    }

    protected override IQueryable<Customer> GetBaseQuery()
    {
        return context.Customers.AsQueryable();
    }

    protected override IQueryable<Customer> AddQueryArguments(
        SearchableCustomer searchable,
        IQueryable<Customer> query)
    {
        if (!string.IsNullOrWhiteSpace(searchable.Name))
            query = query.Where(x => x.Name.ToLower() == searchable.Name.ToLower());

        if (!string.IsNullOrWhiteSpace(searchable.Phone))
            query = query.Where(x => x.Phone.ToLower() == searchable.Phone.ToLower());

        if (!string.IsNullOrWhiteSpace(searchable.Email))
            query = query.Where(x => x.Email.ToLower() == searchable.Email.ToLower());

        return query;
    }
}
