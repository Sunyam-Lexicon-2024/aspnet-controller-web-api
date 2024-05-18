using Microsoft.EntityFrameworkCore;

namespace ControllerWebAPI.Models;

public class ApiDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Item> Items { get; set; } = null!;
}
