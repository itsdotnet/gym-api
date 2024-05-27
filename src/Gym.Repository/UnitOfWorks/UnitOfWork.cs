using Gym.Domain.Entities;
using Gym.Repository.Contexts;
using Gym.Repository.Repositories;

namespace Gym.Repository.UnitOfWorks;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _dbContext;
    
    public UnitOfWork(AppDbContext dbContext)
    {
        _dbContext = dbContext;

        UserRepository = new GRepository<User>(dbContext);
        VideoRepository = new GRepository<Video>(dbContext);
        CourseRepository = new GRepository<Course>(dbContext);
        AttachmentRepository = new GRepository<Attachment>(dbContext);
    }

    public IGRepository<User> UserRepository { get; }
    public IGRepository<Video> VideoRepository { get; }
    public IGRepository<Course> CourseRepository { get; }
    public IGRepository<Attachment> AttachmentRepository { get; }
    
    public void Dispose()
    { GC.SuppressFinalize(true); }
    
    public async Task<bool> SaveAsync()
    {
        var result = await _dbContext.SaveChangesAsync();
        return result > 0;
    }
}