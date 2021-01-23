using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Ordering.Application.Mapper;
using Ordering.Application.Queries;
using Ordering.Application.Responses;
using Ordering.Core.Repositories;
using Ordering.Infrastructure.Data;

namespace Ordering.Application.Handlers
{
    public class GetOrderByUserNameHandler : IRequestHandler<GetOrderByUserNameQuery, IEnumerable<OrderResponse>>
    {
        private readonly IOrderRepository _orderRepository;

        public GetOrderByUserNameHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<IEnumerable<OrderResponse>> Handle(GetOrderByUserNameQuery request,
            CancellationToken cancellationToken)
        {
            var orderList = await _orderRepository.GetOrdersByUserName(request.UserName);
            var orderResponseList = OrderMapper.Mapper.Map<IEnumerable<OrderResponse>>(orderList);
            return orderResponseList;
        }
    }
}