using System;
namespace MarketDataProvider.Services.Models
{
    public class OrderBookTuple
    {
        public decimal Price { get; set; }
        public decimal Orders { get; set; }

        public OrderBookTuple(decimal price, decimal orders)
        {
            Price = price;
            Orders = orders;
        }
    }
}
