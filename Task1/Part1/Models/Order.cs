namespace Part1.Models;

public class Order
{
    public string OrderId { get; init; }
    public List<OrderItem> MenuItems { get; init; }
}
