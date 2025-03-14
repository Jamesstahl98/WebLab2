using Microsoft.EntityFrameworkCore;
using WebLab2.Entities;

namespace WebLab2.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
}
