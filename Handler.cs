using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ManagermentShopDemo
{
    public class Handler
    {
        public static List<Product> _products = new List<Product>();
        public static List<Bill> _bills = new List<Bill>();
        public static List<Report> _reports = new List<Report>();
        public static decimal _totalOutCome = 45000;
        public static List<Product> CreateProduct(List<Product> products) 
        {
            _products.AddRange(products);

            return _products;
        }

        public static bool UpdateProduct(Product item)
        {
            var product = _products.FirstOrDefault(x=>x.Id == item.Id);

            if (product is null)
                return false;

            product.Price = item.Price;
            product.TotalQuantity = item.TotalQuantity;
            product.QuantityRemain = item.QuantityRemain;
            product.Name = item.Name;
            product.UpdatedDate = DateTime.Now;

            return true;
        }

        public static Bill CreateBill(Bill bill) 
        {
            var products = bill.ProductItems.ToList();

            var productIds = products.Select(p=>p.BaseProduct.Id).ToList();
            var baseProduct = _products.Where(x => productIds.Contains(x.Id));
            foreach (var product in baseProduct) 
            {
                var productById = products.FirstOrDefault(x=>x.BaseProduct.Id == product.Id);
                if (productById is null)
                    continue;

                if (productById.Quantity > product.QuantityRemain)
                {
                    productById.Quantity = product.QuantityRemain;
                    product.QuantityRemain = 0;
                }
                else
                {
                    product.QuantityRemain -= productById.Quantity;
                }
            }

            _bills.Add(bill);

            return bill;
        }

        public static bool UpdateBillStatus(Guid Id, PayStatus payStatus)
        { 
            var bill = _bills.FirstOrDefault(x=>x.Id == Id);

            if (bill is null) 
                return false;

            bill.Status = payStatus;
            bill.UpdatedDate = DateTime.Now;
            return true;
        }

        public static bool CancelBill(Guid id, string reason) 
        {
            var bill = _bills.FirstOrDefault(b => b.IsCancel == false && b.Id == id);

            if (bill is null) 
                return false;

            bill.IsCancel = true;

            bill.ReasonCancel = reason; 

            return true;
        }

        public static Report? ShowReport(
            DateTime fromDate,
            DateTime toDate, 
            PayStatus? status = null) 
        {
            var isIgnoreBillStatus = status is null;

            var bills = new List<Bill>();

            if (isIgnoreBillStatus)
            {
                bills = _bills.Where(b =>
                                     b.CreatedDate >= fromDate &&
                                     b.CreatedDate <= toDate).ToList();
            }
            else
            {
                bills = _bills.Where(b =>
                        b.CreatedDate >= fromDate &&
                        b.CreatedDate <= toDate &&
                        b.Status == status).ToList();
            }

            if (bills.Count == 0)
                return null;

            var products = _products.SelectMany(p => bills.SelectMany(x => x.ProductItems));

            decimal totalPrice = 0;

            foreach (var product in products)
                totalPrice += product.TotalPrice();

            return new Report
            {
                FromDate = fromDate,
                ToDate = toDate,
                Products = products.ToList(),
                TotalPrice = totalPrice
            };
        }

        public static string FormatCurrency(decimal value)
        {
            CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");   // try with "en-US"
            if (decimal.TryParse(value.ToString(), out decimal result))
                return result.ToString("#,###", cul.NumberFormat);
    
            return value.ToString();
        }
    }
}
