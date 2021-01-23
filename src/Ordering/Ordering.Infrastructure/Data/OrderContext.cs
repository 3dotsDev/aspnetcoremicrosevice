using Microsoft.EntityFrameworkCore;
using Ordering.Core.Entities;

namespace Ordering.Infrastructure.Data
{
    public class OrderContext : DbContext
    {
        // -s startup-project set to API where inject the context
        // -p where to create the migrations
        // -c context to use for the command
        
        // cd src
        // cd Ordering
        // dotnet ef migrations add InitialCreate -c OrderContext -p Ordering.Infrastructure -s Ordering.API
        // dotnet ef database update -c OrderContext -p Ordering.Infrastructure -s Ordering.API
        public OrderContext(DbContextOptions<OrderContext> options) : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; }
    }
}