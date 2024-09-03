using ManagermentShopDemo;

var createOrder = Handler.CreateProduct(new List<Product>
{
    new Product
    {
        Name = "Milk Coffe",
        Price = 20000,
        QuantityRemain = 1,
        TotalQuantity = 1
    },
    new Product
    {
        Name = "Black Coffe",
        Price = 15000,
        QuantityRemain = 1,
        TotalQuantity = 1
    },
    new Product
    {
        Name = "Milk Tea",
        Price = 25000,
        QuantityRemain = 1,
        TotalQuantity = 1
    },
});

if(Handler._products.Count > 0)
{
    Console.WriteLine("List Product:");
    foreach (var item in Handler._products)
    {
        Console.WriteLine("Product: {0}, Price: {1}", item.Name, Handler.FormatCurrency(item.Price));
    }
}

var firstItem = Handler._products.FirstOrDefault(x => x.Name == "Milk Coffe");
var seccondItem = Handler._products.FirstOrDefault(x => x.Name == "Black Coffe");

if(firstItem is not null && seccondItem is not null)
{
    var createBill = Handler.CreateBill(new Bill
    {
        ProductItems = new List<ProductOrder>
    {
        new ProductOrder
        {
            BaseProduct = firstItem,
            Price = firstItem.Price,
            Quantity = 1
        },
        new ProductOrder
        {
            BaseProduct = seccondItem,
            Price = seccondItem.Price,
            Quantity = 2
        },
    },
        Status = PayStatus.Paid
    });

    if (createBill is not null)
    {
        Console.WriteLine("-------------------------------------");
        Console.WriteLine("Create Bill:");
        int i = 0;
        foreach (var billItem in createBill.ProductItems)
        {
            i++;
            Console.WriteLine("Name: {0}, Price: {1}, Quantity: {2}", billItem.BaseProduct.Name, Handler.FormatCurrency(billItem.TotalPrice()), billItem.Quantity);
        }
        Console.WriteLine("TotalPrice: {0}", Handler.FormatCurrency(createBill.TotalPrice()));
        Console.WriteLine("Bill status: {0}", createBill.Status);
    }
}

var fromDate = DateTime.Now.AddDays(-1);
var toDate = DateTime.Now;
PayStatus? payStatus = PayStatus.Paid;
var stringTextPayStatus = payStatus is null ? "all" : Enum.GetName(payStatus.Value);
var report = Handler.ShowReport(fromDate, toDate, payStatus);

var totalRevenue = report is null ? 0 : report.TotalPrice;
if (report is not null)
{
    Console.WriteLine("-------------------------------------");
    Console.WriteLine($"List Report from {fromDate} to {toDate} with bill status is {stringTextPayStatus}:");
    foreach (var product in report.Products)
    {
        Console.WriteLine("Product info: Name: {0}, Quantity: {1}, Price: {2}, TotalUnitPrice: {3}", product.BaseProduct.Name, product.Quantity, Handler.FormatCurrency(product.Price), Handler.FormatCurrency(product.TotalPrice()));
    }

    Console.WriteLine("TotalRevenue: {0}", Handler.FormatCurrency(totalRevenue));
}
else 
{
    Console.WriteLine("-------------------------------------");
    Console.WriteLine($"List Report from {fromDate} to {toDate} with bill status is {payStatus}:");
    Console.WriteLine("Not found any report with situable for you condition");
}

Console.WriteLine("Now outcome: {0}",Handler.FormatCurrency(Handler._totalOutCome));
Console.WriteLine("Total Profit: {0}", Handler.FormatCurrency(totalRevenue - Handler._totalOutCome));

Console.WriteLine("---------------------------");
Console.WriteLine("List product quantity present: ");
foreach(var product in Handler._products)
{
    Console.WriteLine("Product: {0}, Remain quantity: {1}", product.Name, product.QuantityRemain);
}

Console.ReadLine();