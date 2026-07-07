using Application.DTOs;
using Application.Orders.Queries;
using Domain.Interfaces;
using MediatR;

namespace Application.Orders;

public class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, List<OrderDto>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly ITenantProvider _tenantProvider;

    public GetOrdersQueryHandler(IOrderRepository orderRepository, ITenantProvider tenantProvider)
    {
        _orderRepository = orderRepository;
        _tenantProvider = tenantProvider;
    }

    public async Task<List<OrderDto>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        var orders = await _orderRepository.GetByTenantAsync(
            _tenantProvider.TenantId,
            cancellationToken);

        return orders
            .Select(order => new OrderDto(
                order.Id,
                order.Status.ToString(),
                order.CreatedAt,
                order.CustomerName,
                order.TotalAmount))
            .ToList();
    }
}
