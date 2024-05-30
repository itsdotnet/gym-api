using Gym.Domain.Entities;
using Gym.Service.DTOs.Attachments;

namespace Gym.Service.Interfaces;

public interface IAttachmentService
{
    Task<bool> DeleteAsync(long id);
    Task<Attachment> UploadAsync(AttachmentCreationDto dto, string folder);
}