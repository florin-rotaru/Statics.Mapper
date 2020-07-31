using System;
using System.Collections.Generic;

namespace Models
{
    public class Address
    {
        public Guid Id { get; set; }

        public string Alias { get; set; }
        public string Contact { get; set; }
        public string Phone { get; set; }
        public string Region { get; set; }
        public string Street { get; set; }
    }

    public class AddressDto
    {
        public string Alias { get; set; }
        public string Contact { get; set; }
        public string Phone { get; set; }
        public string Region { get; set; }
        public string Street { get; set; }
    }

    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal ShelfPrice { get; set; }
    }

    public class ProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal ShelfPrice { get; set; }
    }

    public class OrderItem
    {
        public Guid OrderId { get; set; }
        public Guid Id { get; set; }
        public Product Product { get; set; }
        public decimal Quantity { get; set; }
        public decimal Discount { get; set; }
        public decimal FinalPrice { get; set; }
    }

    public class OrderItemDto
    {
        public Guid Id { get; set; }
        public ProductDto Product { get; set; }
        public decimal Quantity { get; set; }
        public decimal Discount { get; set; }
        public decimal FinalPrice { get; set; }
    }

    public class Order
    {
        public Address DeliveryAddresses { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime EstimatedDeliveryDate { get; set; }

        public List<OrderItem> Items { get; set; }
    }

    public class OrderDto
    {
        public AddressDto DeliveryAddresses { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime EstimatedDeliveryDate { get; set; }

        public List<OrderItemDto> Items { get; set; }
    }


    public class Account
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public string Email { get; set; }
        public string Phone { get; set; }

        public string WebSite { get; set; }
        public string ImageUrl { get; set; }



        public Address BillingAddress1 { get; set; }
        public Address BillingAddress2 { get; set; }
        public Address BillingAddress3 { get; set; }
        public Address BillingAddress4 { get; set; }
        public Address BillingAddress5 { get; set; }
        public Address BillingAddress6 { get; set; }
        public Address BillingAddress7 { get; set; }
        public Address BillingAddress8 { get; set; }
        public Address BillingAddress9 { get; set; }


        public OrderItem OrderItem1 { get; set; }
        public OrderItem OrderItem2 { get; set; }
        public OrderItem OrderItem3 { get; set; }
        public OrderItem OrderItem4 { get; set; }
        public OrderItem OrderItem5 { get; set; }
        public OrderItem OrderItem6 { get; set; }
        public OrderItem OrderItem7 { get; set; }
        public OrderItem OrderItem8 { get; set; }
        public OrderItem OrderItem9 { get; set; }


        public List<Address> DeliveryAddresses1 { get; set; }
        public List<Address> DeliveryAddresses2 { get; set; }
        public List<Address> DeliveryAddresses3 { get; set; }
        public List<Address> DeliveryAddresses4 { get; set; }
        public List<Address> DeliveryAddresses5 { get; set; }
        public List<Address> DeliveryAddresses6 { get; set; }
        public List<Address> DeliveryAddresses7 { get; set; }
        public List<Address> DeliveryAddresses8 { get; set; }
        public List<Address> DeliveryAddresses9 { get; set; }



        public Address BillingAddress { get; set; }

        public Address DefaultDeliveryAddress { get; set; }
        public List<Address> DeliveryAddresses { get; set; }

        public List<Order> Orders { get; set; }

        public List<OrderItem> Items { get; set; }
    }

    public class AccountDto
    {
        //public Guid Id { get; set; }
        //public string Name { get; set; }

        //public string WebSite { get; set; }
        //public string ImageUrl { get; set; }



        //public AddressDto BillingAddress1 { get; set; }
        //public AddressDto BillingAddress2 { get; set; }
        //public AddressDto BillingAddress3 { get; set; }
        //public AddressDto BillingAddress4 { get; set; }
        //public AddressDto BillingAddress5 { get; set; }
        //public AddressDto BillingAddress6 { get; set; }
        //public AddressDto BillingAddress7 { get; set; }
        //public AddressDto BillingAddress8 { get; set; }
        //public AddressDto BillingAddress9 { get; set; }

        //public OrderItemDto OrderItem1 { get; set; }
        //public OrderItemDto OrderItem2 { get; set; }
        //public OrderItemDto OrderItem3 { get; set; }
        //public OrderItemDto OrderItem4 { get; set; }
        //public OrderItemDto OrderItem5 { get; set; }
        //public OrderItemDto OrderItem6 { get; set; }
        //public OrderItemDto OrderItem7 { get; set; }
        //public OrderItemDto OrderItem8 { get; set; }
        //public OrderItemDto OrderItem9 { get; set; }


        //public List<AddressDto> DeliveryAddresses1 { get; set; }
        //public List<AddressDto> DeliveryAddresses2 { get; set; }
        //public List<AddressDto> DeliveryAddresses3 { get; set; }
        //public List<AddressDto> DeliveryAddresses4 { get; set; }
        //public List<AddressDto> DeliveryAddresses5 { get; set; }
        //public List<AddressDto> DeliveryAddresses6 { get; set; }
        //public List<AddressDto> DeliveryAddresses7 { get; set; }
        //public List<AddressDto> DeliveryAddresses8 { get; set; }
        //public List<AddressDto> DeliveryAddresses9 { get; set; }


        //public AddressDto BillingAddress { get; set; }
        //public AddressDto DefaultDeliveryAddress { get; set; }
        //public List<AddressDto> DeliveryAddresses { get; set; }

        public List<OrderDto> Orders { get; set; }

        //public List<OrderItemDto> Items { get; set; }
    }
}
