using MediatR;
using Application.DTOs;

namespace Application.Orders.Commands;

public record CreateOrderCommand(
    string CustomerName,
    decimal TotalAmount
) : IRequest<OrderDto>;