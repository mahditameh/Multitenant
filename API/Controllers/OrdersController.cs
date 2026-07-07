using Application.DTOs;
using Application.Orders.Commands;
using Application.Orders.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrdersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(typeof(OrderDto), StatusCodes.Status201Created)]
    public async Task<ActionResult<OrderDto>> Create(
        [FromBody] CreateOrderRequest request,
        CancellationToken cancellationToken)
    {
        var order = await _mediator.Send(
            new CreateOrderCommand(request.CustomerName, request.TotalAmount),
            cancellationToken);

        return Created($"/api/orders/{order.Id}", order);
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<OrderDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<OrderDto>>> Get(CancellationToken cancellationToken)
    {
        var orders = await _mediator.Send(new GetOrdersQuery(), cancellationToken);

        return Ok(orders);
    }
}

public record CreateOrderRequest(
    string CustomerName,
    decimal TotalAmount);
