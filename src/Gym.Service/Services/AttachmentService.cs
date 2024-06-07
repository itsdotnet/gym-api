using Gym.Domain.Entities;
using Gym.Repository.Repositories;
using Gym.Service.DTOs.Attachments;
using Gym.Service.Exceptions;
using Gym.Service.Extensions;
using Gym.Service.Helpers;
using Gym.Service.Interfaces;

namespace Gym.Service.Services;


public class AttachmentService : IAttachmentService
{
    private readonly IGRepository<Attachment> _attachmentRepository;

    public AttachmentService(IGRepository<Attachment> repository)
    {
        _attachmentRepository = repository;
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var attachment = await _attachmentRepository.SelectAsync(x => x.Id == id);

        if (attachment is null)
            throw new NotFoundException("Attachment not found");

        await _attachmentRepository.DeleteAsync(x => x == attachment);
        await _attachmentRepository.SaveAsync();

        return true;
    }

    public async Task<Attachment> UploadAsync(AttachmentCreationDto dto,string folder)
    {
        var webrootPath = Path.Combine(PathHelper.WebRootPath, folder);

        if (!Directory.Exists(webrootPath))
            Directory.CreateDirectory(webrootPath);

        var fileName = "";

        if (folder == "videos")
            fileName = MediaHelper.MakeVideoName(dto.File.FileName);    
        else
            fileName = MediaHelper.MakeImageName(dto.File.FileName);
        
        var fullPath = Path.Combine(webrootPath, fileName);

        var fileStream = new FileStream(fullPath, FileMode.OpenOrCreate);
        await fileStream.WriteAsync(dto.File.ToByte());

        var createdAttachment = new Attachment
        {
            FileName = fileName,
            FilePath = fullPath
        };

        await _attachmentRepository.AddAsync(createdAttachment);
        await _attachmentRepository.SaveAsync();

        return createdAttachment;
    }
}