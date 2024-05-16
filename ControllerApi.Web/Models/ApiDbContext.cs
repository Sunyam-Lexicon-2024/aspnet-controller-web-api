using Microsoft.EntityFrameworkCore;

namespace ControllerApi.Web.Models;

public class ApiDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Item> Items { get; set; } = null!;
}
