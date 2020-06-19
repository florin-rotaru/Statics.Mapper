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
        public decimal? Discount { get; set; }
        public decimal FinalPrice { get; set; }
    }

    public enum OrderStatus
    {
        Canceled = -1,
        Undefined = 0,
        Pending = 1,
        InProgress = 2,
        Completed = 3
    }

    public class Order
    {
        public Account BillingAccount { get; set; }
        public Account DeliveryAccount { get; set; }

        public OrderStatus Status { get; set; }
        public OrderItem[] Items { get; set; }
    }

    public class OrderDto
    {
        public AccountDto BillingAccount { get; set; }
        public AccountDto DeliveryAccount { get; set; }

        public string Status { get; set; }
        public OrderItem[] Items { get; set; }
    }

    public class Account
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public string Email { get; set; }
        public string Phone { get; set; }

        public Address BillingAddress { get; set; }

        public Address DefaultDeliveryAddress { get; set; }
        public Address[] DeliveryAddresses { get; set; }

        public Order[] Orders { get; set; }
    }

    public class AccountDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public AddressDto DefaultDeliveryAddress { get; set; }
        public AddressDto[] DeliveryAddresses { get; set; }

        public OrderDto[] Orders { get; set; }
    }
}
