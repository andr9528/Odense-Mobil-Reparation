using Newtonsoft.Json;
using Repair.Abstractions.Entity.Model;

namespace Repair.Models.Entity.Model;

public class Order : IOrder
{
    private int id;

    public int Id
    {
        get => id;
        set => throw new InvalidOperationException(
            $"{nameof(Id)} cannot be changed after creation of {nameof(Order)} entity");
    }

    public byte[] Version { get; set; } = [];

    public DateTime CreatedDateTime { get; set; }

    public DateTime UpdatedDateTime { get; set; }

    public string HandInWhat { get; set; } = string.Empty;

    public string RepairWhat { get; set; } = string.Empty;

    public int CustomerId { get; set; }

    public DateTime HandInWhen { get; set; }

    public DateTime? ReturnedWhen { get; set; }

    public bool IsOrderComplete { get; set; }

    public bool HasBorrowedPhone { get; set; }

    public ICustomer Customer { get; set; } = null!;

    [JsonConstructor]
    private Order(int id, Customer customer)
    {
        this.id = id;
        Customer = customer;
    }

    public Order()
    {
    }
}
