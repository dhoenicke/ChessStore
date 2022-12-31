#pragma warning disable CS8618
using Microsoft.EntityFrameworkCore;
namespace ChessStore.Models;
public class MyContext : DbContext
{
    public MyContext(DbContextOptions options) : base(options) { }
    public DbSet<User> Users { get; set; }
    public DbSet<Board> Boards { get; set; }
    public DbSet<Order> Orders { get; set; }
}