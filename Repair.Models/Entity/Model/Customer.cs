using Newtonsoft.Json;
using Repair.Abstractions.Entity.Model;

namespace Repair.Models.Entity.Model;

public class Customer : ICustomer
{
    private int id;

    public int Id
    {
        get => id;
        set => throw new InvalidOperationException(
            $"{nameof(Id)} cannot be changed after creation of {nameof(Customer)} entity");
    }

    public byte[] Version { get; set; } = [];

    public DateTime CreatedDateTime { get; set; }

    public DateTime UpdatedDateTime { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public ICollection<IOrder> Orders { get; set; } = new List<IOrder>();

    [JsonConstructor]
    private Customer(int id, List<Order> orders)
    {
        this.id = id;
        Orders = orders.Cast<IOrder>().ToList();
    }

    public Customer()
    {
    }
}
