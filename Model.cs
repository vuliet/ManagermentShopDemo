using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ManagermentShopDemo
{
    public class Product
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Name { get; set; }

        public int TotalQuantity { get; set; }

        public int QuantityRemain { get; set; }

        public decimal Price { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime? UpdatedDate { get; set; }
    }

    public class ProductOrder
    {
        public Product BaseProduct { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice ()
        {
            return Quantity * Price;
        }
    }

    public class Bill
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid CustomerId { get; set; } = new Guid();

        public List<ProductOrder> ProductItems { get; set; }

        public PayStatus Status { get; set; } = PayStatus.Open;

        public PayType PayType { get; set; } = PayType.Cash;

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime? UpdatedDate { get; set; }

        public bool IsCancel { get; set; } = false;

        public string ReasonCancel { get; set; } = string.Empty;

        public decimal TotalPrice()
        {
            decimal totalPrice = 0;
            foreach(var item in ProductItems)
                totalPrice += item.Price*item.Quantity;

            return totalPrice;
        }
    }

    public class Customer 
    { 
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Name { get; set; } = "Customer_" + DateTime.Now;

        public DateTime CheckInTime { get; set; } = DateTime.Now;
    }

    public class Report
    {
        public decimal TotalPrice { get; set; }

        public List<ProductOrder> Products { get; set; }

        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }

    public enum PayType
    {
        Cash = 0,
        ATM_VISA = 1,
        Transfer = 2
    }

    public enum PayStatus
    {
        Open = 0,
        Pending = 1,
        Paid = 2 
    }
}
