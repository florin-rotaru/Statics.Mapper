using Mapster;
using Mapster.Utils;
using Models;

namespace Benchmark
{
    public static class MapsterAccountMapper
    {
        public static AccountDto Map(Account p1)
        {
            return p1 == null ? null : new AccountDto()
            {
                Id = p1.Id,
                Name = p1.Name,
                DefaultDeliveryAddress = p1.DefaultDeliveryAddress == null ? null : new AddressDto()
                {
                    Alias = p1.DefaultDeliveryAddress.Alias,
                    Contact = p1.DefaultDeliveryAddress.Contact,
                    Phone = p1.DefaultDeliveryAddress.Phone,
                    Region = p1.DefaultDeliveryAddress.Region,
                    Street = p1.DefaultDeliveryAddress.Street
                },
                DeliveryAddresses = func1(p1.DeliveryAddresses),
                Orders = func2(p1.Orders)
            };
        }

        private static AddressDto[] func1(Address[] p2)
        {
            if (p2 == null)
            {
                return null;
            }
            AddressDto[] result = new AddressDto[p2.Length];

            int v = 0;

            int i = 0;
            int len = p2.Length;

            while (i < len)
            {
                Address item = p2[i];
                result[v++] = item == null ? null : new AddressDto()
                {
                    Alias = item.Alias,
                    Contact = item.Contact,
                    Phone = item.Phone,
                    Region = item.Region,
                    Street = item.Street
                };
                i++;
            }
            return result;

        }

        private static OrderDto[] func2(Order[] p3)
        {
            if (p3 == null)
            {
                return null;
            }
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
                BillingAccount = TypeAdapter<Account, AccountDto>.Map.Invoke(p4.BillingAccount),
                DeliveryAccount = TypeAdapter<Account, AccountDto>.Map.Invoke(p4.DeliveryAccount),
                Status = Enum<OrderStatus>.ToString(p4.Status),
                Items = func4(p4.Items)
            };
        }

        private static OrderItem[] func4(OrderItem[] p5)
        {
            if (p5 == null)
            {
                return null;
            }
            OrderItem[] result = new OrderItem[p5.Length];

            int v = 0;

            int i = 0;
            int len = p5.Length;

            while (i < len)
            {
                OrderItem item = p5[i];
                result[v++] = item == null ? null : new OrderItem()
                {
                    OrderId = item.OrderId,
                    Id = item.Id,
                    Product = item.Product == null ? null : new Product()
                    {
                        Id = item.Product.Id,
                        Name = item.Product.Name,
                        Department = item.Product.Department,
                        PurchasePrice = item.Product.PurchasePrice,
                        ShelfPrice = item.Product.ShelfPrice
                    },
                    Quantity = item.Quantity,
                    Discount = item.Discount,
                    FinalPrice = item.FinalPrice
                };
                i++;
            }
            return result;

        }
    }
}