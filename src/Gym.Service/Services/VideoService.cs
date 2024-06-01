using AutoMapper;
using Gym.Domain.Entities;
using Gym.Repository.UnitOfWorks;
using Gym.Service.DTOs.Attachments;
using Gym.Service.DTOs.Videos;
using Gym.Service.Exceptions;
using Gym.Service.Helpers;
using Gym.Service.Interfaces;

namespace Gym.Service.Services;

public class VideoService : IVideoService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IAttachmentService _attachmentService;

    public VideoService(IUnitOfWork unitOfWork, IMapper mapper, IAttachmentService attachmentService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _attachmentService = attachmentService;
    }
    
    public async Task<bool> DeleteAsync(long id)
    {
        var video = await _unitOfWork.VideoRepository.SelectAsync(q => q.Id == id);

        if (video is null)
            throw new NotFoundException("Video not found");

        await _unitOfWork.VideoRepository.DeleteAsync(x => x == video);
        return await _unitOfWork.SaveAsync();
    }

    public async Task<VideoResultDto> GetByIdAsync(long id)
    {
        
        var video = await _unitOfWork.VideoRepository.SelectAsync(q => q.Id == id, new []{"Attachment"});

        if (video is null)
            throw new NotFoundException("Video not found");
        
        return _mapper.Map<VideoResultDto>(video);
    }

    public async Task<IEnumerable<VideoResultDto>> GetAllAsync()
    {
        var videos = (IEnumerable<Video>)_unitOfWork.VideoRepository.SelectAll(includes: new []{"Attachment"});

        return _mapper.Map<IEnumerable<VideoResultDto>>(videos);
    }

    public async Task<IEnumerable<VideoResultDto>> GetByCourseAsync(long courseId)
    {
        var videos = (IEnumerable<Video>)_unitOfWork.VideoRepository
            .SelectAll(x => x.CourseId == courseId, includes: new []{"Attachment"});

        return _mapper.Map<IEnumerable<VideoResultDto>>(videos);
    }

    public async Task<VideoResultDto> CreateAsync(VideoCreationDto dto)
    {
        var video = new Attachment();
        var course = await _unitOfWork.CourseRepository.SelectAsync(x => x.Id == dto.CourseId);
        if (course is null)
            throw new NotFoundException("Course not found");
        
        if (dto.Video is null)
            throw new ArgumentNullException();
        else
        {
            if (Validator.IsVideo(dto.Video.FileName))
            {
                video = await _attachmentService
                    .UploadAsync(new AttachmentCreationDto() { File = dto.Video }, "videos");
            }
            else
            {
                throw new CustomException(400, "This file is not video");
            }
        }
        var newVideo = _mapper.Map<Video>(dto);
        newVideo.AttachmentId = video.Id;
        await _unitOfWork.VideoRepository.AddAsync(newVideo);
        await _unitOfWork.SaveAsync();

        return _mapper.Map<VideoResultDto>(newVideo);
    }

    public async Task<VideoResultDto> UpdateAsync(VideoUpdateDto dto)
    {
        
        var video = new Attachment();
        var exist = await _unitOfWork.VideoRepository.SelectAsync(d => d.Id == dto.Id);
    
        var course = await _unitOfWork.CourseRepository.SelectAsync(x => x.Id == dto.CourseId);
        if (course is null)
            throw new NotFoundException("Course not found");

        if (exist is null)
            throw new NotFoundException("Video not found");
        if (dto.Name == exist.Name && dto.Description == exist.Description && dto.CourseId == exist.CourseId)
            if(dto.Video is null)
                throw new CustomException(400, "You changed nothing");
        
        if (Validator.IsVideo(dto.Video.FileName))
            video = await _attachmentService
                .UploadAsync(new AttachmentCreationDto() { File = dto.Video }, "videos");
        else
            throw new CustomException(400, "This file is not video");
        
        _mapper.Map(dto, exist);
        exist.AttachmentId = video.Id;

        await _unitOfWork.VideoRepository.UpdateAsync(exist);
        await _unitOfWork.SaveAsync();

        return _mapper.Map<VideoResultDto>(exist);
    }
}