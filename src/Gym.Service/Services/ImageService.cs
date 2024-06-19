using AutoMapper;
using Gym.Domain.Entities;
using Gym.Repository.UnitOfWorks;
using Gym.Service.DTOs.Attachments;
using Gym.Service.DTOs.Images;
using Gym.Service.DTOs.Users;
using Gym.Service.Exceptions;
using Gym.Service.Helpers;
using Gym.Service.Interfaces;

namespace Gym.Service.Services;

public class ImageService : IImageService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IAttachmentService _attachmentService;

    public ImageService(IUnitOfWork unitOfWork, IMapper mapper, IAttachmentService attachmentService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _attachmentService = attachmentService;
    }
    
    public async Task<bool> DeleteAsync(long id)
    {
        var image = await _unitOfWork.ImageRepository.SelectAsync(q => q.Id == id);

        if (image is null)
            throw new NotFoundException("Image not found");

        await _unitOfWork.ImageRepository.DeleteAsync(x => x == image);
        return await _unitOfWork.SaveAsync();
    }

    public async Task<ImageResultDto> GetByIdAsync(long id)
    {
        var image = await _unitOfWork.ImageRepository.SelectAsync(q => q.Id == id, new []{"Attachment"});

        if (image is null)
            throw new NotFoundException("Image not found");
        
        return _mapper.Map<ImageResultDto>(image);
    }

    public async Task<IEnumerable<ImageResultDto>> GetAllAsync()
    {
        var images = (IEnumerable<Image>)_unitOfWork.ImageRepository.SelectAll(includes: new []{"Attachment"});

        return _mapper.Map<IEnumerable<ImageResultDto>>(images);
    }

    public async Task<ImageResultDto> CreateAsync(ImageCreationDto dto)
    {
        var image = new Attachment();
        if (dto.Image is null)
            throw new ArgumentNullException();
        else
        {
            if (Validator.IsImage(dto.Image.FileName))
            {
                image = await _attachmentService
                    .UploadAsync(new AttachmentCreationDto() { File = dto.Image }, "images");
            }
            else
            {
                throw new CustomException(400, "This file is not image");
            }
        }

        var newImage = _mapper.Map<Image>(dto);
        newImage.AttachmentId = image.Id;
        await _unitOfWork.ImageRepository.AddAsync(newImage);
        await _unitOfWork.SaveAsync();

        return _mapper.Map<ImageResultDto>(newImage);
    }

    public async Task<ImageResultDto> UpdateAsync(ImageUpdateDto dto)
    {
        var image = new Attachment();
        var exist = await _unitOfWork.ImageRepository.SelectAsync(d => d.Id == dto.Id);
    
        if (exist is null)
            throw new NotFoundException("Image not found");
        if (dto.Name == exist.Name && dto.Description == exist.Description && dto.Image is null)
            if(dto.Image is null)
                throw new CustomException(400, "You changed nothing");
        if(dto.Image is null)
        {
            if (Validator.IsImage(dto.Image.FileName))
                image = await _attachmentService
                    .UploadAsync(new AttachmentCreationDto() { File = dto.Image }, "images");
            else
                throw new CustomException(400, "This file is not image");
            exist.AttachmentId = image.Id;
        }
        _mapper.Map(dto, exist);

        await _unitOfWork.ImageRepository.UpdateAsync(exist);
        await _unitOfWork.SaveAsync();

        return _mapper.Map<ImageResultDto>(exist);
    }
}