

using Domain.Enums;

namespace Domain.Entities;

public class Order
{
    public Guid Id { get; set; }
    public string TenantId { get; set; } = string.Empty;
    public OrderStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }

    public void ChangeStatus(OrderStatus newStatus)
    {
        if (!IsValidTransition(Status, newStatus))
            throw new InvalidOperationException($"Invalid status transition from {Status} to {newStatus}");
        
        Status = newStatus;
        UpdatedAt = DateTime.UtcNow;
    }

    private bool IsValidTransition(OrderStatus from, OrderStatus to)
    {
        return (from, to) switch
        {
            (OrderStatus.Created, OrderStatus.Assigned) => true,
            (OrderStatus.Assigned, OrderStatus.PickedUp) => true,
            (OrderStatus.PickedUp, OrderStatus.Delivered) => true,
            _ => false
        };
    }
}
