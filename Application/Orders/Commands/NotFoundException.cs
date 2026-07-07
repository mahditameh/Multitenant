namespace Application.Orders.Commands;

public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }
}