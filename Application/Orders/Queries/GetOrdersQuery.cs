using MediatR;
using Application.DTOs;

namespace Application.Orders.Queries;

public record GetOrdersQuery() : IRequest<List<OrderDto>>;