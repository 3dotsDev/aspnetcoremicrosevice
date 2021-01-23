using MediatR;
using Ordering.Application.Responses;

namespace Ordering.Application.Commands
{
    // coms from the frontend 
    public class CheckOutOrderCommand : IRequest<OrderResponse>
    {
        public string UserName { get; set; }
        public decimal TotalPrice { get; set; }
        
        //BillingAdress
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EMailAddress { get; set; }
        public string AddressLine { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        
        //Payment
        public string CardName { get; set; }
        public string CardNumber { get; set; }
        public string Expiration { get; set; }
        public string CVV { get; set; }
        public int PaymentMethod { get; set; }
    }
}