using System;
using System.Collections.Generic;

namespace Codetest.Models
{
    public class Pizza
    {
        public int ID { get; set; }
        public string Size { get; set; }
        public double Price { get; set; }
    }

    public class Topping
    {
        public int ID { get; set; }
        public string Type { get; set; }
        public string Item { get; set; }
        public string Size { get; set; }
        public double Price { get; set; }
    }

    public class OrderResponse
    {
        public int ID { get; set; }
        public string Size { get; set; }
        public double Price { get; set; }
        public List<Topping> Toppings { get; set; }
        public double TotalPrice { get; set; }
        public DateTime DateCreated { get; set; }
    }

    public class OrdersResponse
    {
        public List<OrderResponse> Orders { get; set; }
    }

    public class Order
    {
        public int ID { get; set; }
        public Pizza Pizzas { get; set; }
        public List<Topping> Toppings { get; set; }
        public double TotalPrice { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
