using Codetest.Models;
using Codetest.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Codetest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CodetestController : ControllerBase
    {
        [HttpPost]
        [Route("GetOrders")]
        public OrdersResponse GetOrders(int count)
        {
            List<OrderResponse> orders = new List<OrderResponse>();

            foreach (Order item in ProductsDatabase.GetOrders(count))
            {
                orders.Add(new OrderResponse
                {
                    ID = item.ID,
                    Size = item.Pizzas.Size,
                    Price = item.Pizzas.Price,
                    Toppings = item.Toppings,
                    TotalPrice = item.TotalPrice,
                    DateCreated = item.DateCreated
                });
            }

            return new OrdersResponse { Orders = orders };
        }

        [HttpPost]
        [Route("PostOrders")]
        public bool PostOrders(OrderResponse order)
        {
            return ProductsDatabase.PostOrders(order);
        }

        [HttpPost]
        [Route("ClrearOrders")]
        public bool ClrearOrders()
        {
            return ProductsDatabase.ClrearOrders();
        }
    }
}
