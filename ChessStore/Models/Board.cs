#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
namespace ChessStore.Models;
public class Board
{
    [Key]
    public int BoardId { get; set; }
    [Required]
    public string Image { get; set; }
    [Required]
    public string Title { get; set; }
    [Required]
    [Range(0.01, double.MaxValue)]
    public double Price { get; set; }
    [Required]
    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }
    [Required]
    [Range(1, 5)]
    public int Rating { get; set; }
    [Required]
    [MinLength(20, ErrorMessage = "Description must be greater than 20 characters")]
    public string Description { get; set; }

    // This is for my one to many
    [Required]
    public int UserId { get; set; }
    public User? Creator { get; set; }

    // This is for my many to many
    public List<Order> OrdersPlaced { get; set; } = new List<Order>();

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

}