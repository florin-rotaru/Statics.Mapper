
//using System.Collections.Generic;
//using Mapster;
//using Mapster.Utils;
//using Models;


//namespace Benchmark
//{
//    public static partial class CustomerMapper
//    {
//        public static AccountDto Map(Account p1)
//        {
//            return p1 == null ? null : new AccountDto()
//            {
//                Id = p1.Id,
//                Name = p1.Name,
//                DefaultDeliveryAddress = p1.DefaultDeliveryAddress == null ? null : new AddressDto()
//                {
//                    Alias = p1.DefaultDeliveryAddress.Alias,
//                    Contact = p1.DefaultDeliveryAddress.Contact,
//                    Phone = p1.DefaultDeliveryAddress.Phone,
//                    Region = p1.DefaultDeliveryAddress.Region,
//                    Street = p1.DefaultDeliveryAddress.Street
//                },
//                Orders = func2(p1.Orders)
//            };
//        }

//        private static List<AddressDto> func1(List<Address> p2)
//        {
//            if (p2 == null)
//            {
//                return null;
//            }
//            List<AddressDto> result = new List<AddressDto>(p2.Count);

//            int i = 0;
//            int len = p2.Count;

//            while (i < len)
//            {
//                Address item = p2[i];
//                result.Add(item == null ? null : new AddressDto()
//                {
//                    Alias = item.Alias,
//                    Contact = item.Contact,
//                    Phone = item.Phone,
//                    Region = item.Region,
//                    Street = item.Street
//                });
//                i++;
//            }
//            return result;

//        }

//        private static List<OrderDto> func2(List<Order> p3)
//        {
//            if (p3 == null)
//            {
//                return null;
//            }
//            List<OrderDto> result = new List<OrderDto>(p3.Count);

//            int i = 0;
//            int len = p3.Count;

//            while (i < len)
//            {
//                Order item = p3[i];
//                result.Add(func3(item));
//                i++;
//            }
//            return result;

//        }

//        private static OrderDto func3(Order p4)
//        {
//            return p4 == null ? null : new OrderDto()
//            {
//                Items = func4(p4.Items)
//            };
//        }

//        private static List<OrderItemDto> func4(List<OrderItem> p5)
//        {
//            if (p5 == null)
//            {
//                return null;
//            }
//            List<OrderItemDto> result = new List<OrderItemDto>(p5.Count);

//            int i = 0;
//            int len = p5.Count;

//            while (i < len)
//            {
//                OrderItem item = p5[i];
//                result.Add(item == null ? null : new OrderItemDto()
//                {
//                    Id = item.Id,
//                    Product = item.Product == null ? null : new ProductDto()
//                    {
//                        Id = item.Product.Id,
//                        Name = item.Product.Name,
//                        ShelfPrice = item.Product.ShelfPrice
//                    },
//                    Quantity = item.Quantity,
//                    Discount = item.Discount,
//                    FinalPrice = item.FinalPrice
//                });
//                i++;
//            }
//            return result;

//        }
//    }
//}