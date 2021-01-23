using AutoMapper;
using EventBus.RabbitMq.Events;
using Ordering.Application.Commands;

namespace Ordering.API.Mapping
{
    public class OrderMapping : Profile
    {
        public OrderMapping()
        {
            CreateMap<BasketCheckoutEvent, CheckOutOrderCommand>().ReverseMap();
        }
    }
}