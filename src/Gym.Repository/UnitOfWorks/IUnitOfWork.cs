using Gym.Domain.Entities;
using Gym.Repository.Repositories;

namespace Gym.Repository.UnitOfWorks;

public interface IUnitOfWork : IDisposable
{
    IGRepository<User> UserRepository { get; }
    IGRepository<Video> VideoRepository { get; }
    IGRepository<Course> CourseRepository { get; }
    IGRepository<Attachment> AttachmentRepository { get; }
    Task<bool> SaveAsync();
}