using Repair.Models.Entity.Model;

namespace Repair.Tests.Core
{
    public static class EntityFactory
    {
        public static Customer CreateCustomer(string name, string phone, string email)
        {
            return new Customer
            {
                Name = name,
                Phone = phone,
                Email = email,
            };
        }

        public static Order CreateOrder(string handInWhat, string repairWhat, Customer customer)
        {
            return new Order
            {
                HandInWhat = handInWhat,
                RepairWhat = repairWhat,
                Customer = customer,
            };
        }
    }
}
