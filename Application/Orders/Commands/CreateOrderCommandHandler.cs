using MediatR;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Application.DTOs;
using Application.Orders.Commands;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, OrderDto>
{
    private readonly IOrderRepository _orderRepository;
    private readonly ITenantProvider _tenantProvider;

    public CreateOrderCommandHandler(IOrderRepository orderRepository, ITenantProvider tenantProvider)
    {
        _orderRepository = orderRepository;
        _tenantProvider = tenantProvider;
    }

    public async Task<OrderDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = new Order
        {
            Id = Guid.NewGuid(),
            TenantId = _tenantProvider.TenantId,
            Status = OrderStatus.Created,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            CustomerName = request.CustomerName,
            TotalAmount = request.TotalAmount
        };

        _orderRepository.Add(order);
        await _orderRepository.SaveChangesAsync(cancellationToken);

        return new OrderDto(
            order.Id,
            order.Status.ToString(),
            order.CreatedAt,
            order.CustomerName,
            order.TotalAmount
        );
    }
}