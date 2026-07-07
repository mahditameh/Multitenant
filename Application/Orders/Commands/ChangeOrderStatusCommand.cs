    using MediatR;
using Domain.Enums;

namespace Application.Orders.Commands;

public record ChangeOrderStatusCommand(Guid OrderId, OrderStatus NewStatus) : IRequest;