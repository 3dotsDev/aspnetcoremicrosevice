using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ordering.Core.Entities;

namespace Ordering.Infrastructure.Data
{
    public class OrderContextSeed
    {
        public static async Task SeedAsync(OrderContext orderContext, ILoggerFactory loggerFactory, int? retry = 0)
        {
            int retryForAvailability = retry.Value;

            try
            {
                orderContext.Database.Migrate();

                if (!orderContext.Orders.Any())
                {
                    orderContext.Orders.AddRange(GetPreconfiguredOrders());
                    await orderContext.SaveChangesAsync();
                }
            }
            catch (Exception exception)
            {
                if (retryForAvailability < 50)
                {
                    retryForAvailability++;
                    var log = loggerFactory.CreateLogger<OrderContextSeed>();
                    log.LogError(exception.Message);
                    System.Threading.Thread.Sleep(2000);
                    await SeedAsync(orderContext, loggerFactory, retryForAvailability);
                }
            }
        }

        private static IEnumerable<Order> GetPreconfiguredOrders()
        {
            return new List<Order>
            {
                new Order()
                {
                    UserName = "rda", FirstName = "Reto", LastName = "Dalmaso", EMailAddress = "reto.dalmaso@gmail.com",
                    AddressLine = "Wolfsbachstrasse 13", Country = "Switzerland", ZipCode = "9436"
                },
                new Order()
                {
                    UserName = "vsc", FirstName = "Vanessa", LastName = "Schenk",
                    EMailAddress = "vanessa.schenko@gmail.com",
                    AddressLine = "Wolfsbachstrasse 13", Country = "Switzerland", ZipCode = "9436"
                }
            };
        }
    }
}