using Gym.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gym.Repository.Contexts;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    { }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Video> Videos { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Attachment> Attachments { get; set; }
}