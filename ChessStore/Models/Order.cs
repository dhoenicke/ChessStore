#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
namespace ChessStore.Models;
public class Order
{
    [Key]
    public int OrderId { get; set; }

    // The person who ordered the board
    public int UserId { get; set; }
    public User? User { get; set; }

    // The board ordered
    public int BoardId { get; set; }
    public Board? Board { get; set; }

    public int QuantityOrdered { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}