namespace Application.DTOs;

public record OrderDto(
    Guid Id,
    string Status,
    DateTime CreatedAt,
    string CustomerName,
    decimal TotalAmount
);