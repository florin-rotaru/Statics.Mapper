using Mapster.Utils;
using Models;

namespace Benchmark
{
    public static class AirAccountMapper
    {
        public static AccountDto Map(Account p1)
        {
            return p1 == null ? null : new AccountDto()
            {
                Id = p1.Id,
                Name = p1.Name,
                DefaultDeliveryAddress = Air.Mapper.Mapper<Address, AddressDto>.Map(p1.DefaultDeliveryAddress),
                DeliveryAddresses = func1(p1.DeliveryAddresses),
                Orders = func2(p1.Orders)
            };
        }

        private static AddressDto[] func1(Address[] p2)
        {
            if (p2 == null)
                return null;

            AddressDto[] result = new AddressDto[p2.Length];

            for (int i = 0; i < p2.Length; i++)
                result[i] = Air.Mapper.Mapper<Address, AddressDto>.Map(p2[i]);

            return result;

        }

        private static OrderDto[] func2(Order[] p3)
        {
            if (p3 == null)
                return null;

            OrderDto[] result = new OrderDto[p3.Length];

            int v = 0;

            int i = 0;
            int len = p3.Length;

            while (i < len)
            {
                Order item = p3[i];
                result[v++] = func3(item);
                i++;
            }
            return result;

        }

        private static OrderDto func3(Order p4)
        {
            return p4 == null ? null : new OrderDto()
            {
                BillingAccount = Air.Mapper.Mapper<Account, AccountDto>.Map(p4.BillingAccount),
                DeliveryAccount = Air.Mapper.Mapper<Account, AccountDto>.Map(p4.DeliveryAccount),
                Status = Enum<OrderStatus>.ToString(p4.Status),
                Items = func4(p4.Items)
            };
        }

        private static OrderItem[] func4(OrderItem[] p5)
        {
            if (p5 == null)
                return null;

            OrderItem[] result = new OrderItem[p5.Length];

            for (int i = 0; i < p5.Length; i++)
                result[i] = Air.Mapper.Mapper<OrderItem, OrderItem>.Map(p5[i]);

            return result;
        }
    }
}