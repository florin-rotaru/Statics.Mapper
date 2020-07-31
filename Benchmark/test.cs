
//using System.Collections.Generic;
//using Models;


//namespace YourNamespace
//{
//    public static partial class CustomerMapper
//    {
//        public static AccountDto Map(Account p1)
//        {
//            return p1 == null ? null : new AccountDto()
//            {
//                DeliveryAddresses1 = func1(p1.DeliveryAddresses1),
//                DeliveryAddresses2 = func2(p1.DeliveryAddresses2),
//                DeliveryAddresses3 = func3(p1.DeliveryAddresses3),
//                DeliveryAddresses4 = func4(p1.DeliveryAddresses4),
//                DeliveryAddresses5 = func5(p1.DeliveryAddresses5),
//                DeliveryAddresses6 = func6(p1.DeliveryAddresses6),
//                DeliveryAddresses7 = func7(p1.DeliveryAddresses7),
//                DeliveryAddresses8 = func8(p1.DeliveryAddresses8),
//                DeliveryAddresses9 = func9(p1.DeliveryAddresses9)
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

//        private static List<AddressDto> func2(List<Address> p3)
//        {
//            if (p3 == null)
//            {
//                return null;
//            }
//            List<AddressDto> result = new List<AddressDto>(p3.Count);

//            int i = 0;
//            int len = p3.Count;

//            while (i < len)
//            {
//                Address item = p3[i];
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

//        private static List<AddressDto> func3(List<Address> p4)
//        {
//            if (p4 == null)
//            {
//                return null;
//            }
//            List<AddressDto> result = new List<AddressDto>(p4.Count);

//            int i = 0;
//            int len = p4.Count;

//            while (i < len)
//            {
//                Address item = p4[i];
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

//        private static List<AddressDto> func4(List<Address> p5)
//        {
//            if (p5 == null)
//            {
//                return null;
//            }
//            List<AddressDto> result = new List<AddressDto>(p5.Count);

//            int i = 0;
//            int len = p5.Count;

//            while (i < len)
//            {
//                Address item = p5[i];
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

//        private static List<AddressDto> func5(List<Address> p6)
//        {
//            if (p6 == null)
//            {
//                return null;
//            }
//            List<AddressDto> result = new List<AddressDto>(p6.Count);

//            int i = 0;
//            int len = p6.Count;

//            while (i < len)
//            {
//                Address item = p6[i];
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

//        private static List<AddressDto> func6(List<Address> p7)
//        {
//            if (p7 == null)
//            {
//                return null;
//            }
//            List<AddressDto> result = new List<AddressDto>(p7.Count);

//            int i = 0;
//            int len = p7.Count;

//            while (i < len)
//            {
//                Address item = p7[i];
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

//        private static List<AddressDto> func7(List<Address> p8)
//        {
//            if (p8 == null)
//            {
//                return null;
//            }
//            List<AddressDto> result = new List<AddressDto>(p8.Count);

//            int i = 0;
//            int len = p8.Count;

//            while (i < len)
//            {
//                Address item = p8[i];
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

//        private static List<AddressDto> func8(List<Address> p9)
//        {
//            if (p9 == null)
//            {
//                return null;
//            }
//            List<AddressDto> result = new List<AddressDto>(p9.Count);

//            int i = 0;
//            int len = p9.Count;

//            while (i < len)
//            {
//                Address item = p9[i];
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

//        private static List<AddressDto> func9(List<Address> p10)
//        {
//            if (p10 == null)
//            {
//                return null;
//            }
//            List<AddressDto> result = new List<AddressDto>(p10.Count);

//            int i = 0;
//            int len = p10.Count;

//            while (i < len)
//            {
//                Address item = p10[i];
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
//    }
//}