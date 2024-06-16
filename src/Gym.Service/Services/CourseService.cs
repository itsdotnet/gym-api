using AutoMapper;
using Gym.Domain.Entities;
using Gym.Repository.UnitOfWorks;
using Gym.Service.DTOs.Attachments;
using Gym.Service.DTOs.Courses;
using Gym.Service.Exceptions;
using Gym.Service.Helpers;
using Gym.Service.Interfaces;

namespace Gym.Service.Services;

public class CourseService : ICourseService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private IAttachmentService _attachmentService;

    public CourseService(IUnitOfWork unitOfWork, IMapper mapper, IAttachmentService attachmentService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _attachmentService = attachmentService;
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var course = await _unitOfWork.CourseRepository.SelectAsync(q => q.Id == id);

        if (course is null)
            throw new NotFoundException("course not found");

        await _unitOfWork.CourseRepository.DeleteAsync(x => x == course);
        return await _unitOfWork.SaveAsync();
    }

    public async Task<CourseResultDto> GetByIdAsync(long id)
    {
        var course = await _unitOfWork.CourseRepository.SelectAsync(q => q.Id == id,
            includes: new string[] { "Attachment" });

        if (course is null)
            throw new NotFoundException("Course not found");

        return _mapper.Map<CourseResultDto>(course);
    }

    public async Task<IEnumerable<CourseResultDto>> GetAllAsync()
    {
        var courses = _unitOfWork.CourseRepository.SelectAll(includes: new string[] { "Attachment" });
        return _mapper.Map<IEnumerable<CourseResultDto>>(courses);
    }

    public async Task<CourseResultDto> GetByIdWithVideosAsync(long id)
    {
        
        var course = await _unitOfWork.CourseRepository
            .SelectAsync(q => q.Id == id, includes: new string[] { "Attachment" });

        if (course is null)
            throw new NotFoundException("Course not found");

        return _mapper.Map<CourseResultDto>(course);
    }

    public async Task<CourseResultDto> CreateAsync(CourseCreationDto dto)
    {
        var course = _mapper.Map<Course>(dto);
        var image = new Attachment();
        if (dto.Image is null)
            throw new ArgumentNullException();
        
        if (Validator.IsImage(dto.Image.FileName))
            image = await _attachmentService
                .UploadAsync(new AttachmentCreationDto() { File = dto.Image }, "images");
        else
            throw new CustomException(400, "This file is not image");

        course.AttachmentId = image.Id;
        await _unitOfWork.CourseRepository.AddAsync(course);
        await _unitOfWork.SaveAsync();
        
        return _mapper.Map<CourseResultDto>(course);
    }

    public async Task<CourseResultDto> UpdateAsync(CourseUpdateDto dto)
    {
        var existingCourse = await _unitOfWork.CourseRepository.SelectAsync(q => q.Id == dto.Id);
        var image = new Attachment();

        if (existingCourse is null)
            throw new NotFoundException("Course not found");
        if (dto.Image is not null)
        {
            if (Validator.IsImage(dto.Image.FileName))
                image = await _attachmentService
                    .UploadAsync(new AttachmentCreationDto() { File = dto.Image }, "images");
            else
                throw new CustomException(400, "This file is not image");
        }
        _mapper.Map(dto, existingCourse);
        existingCourse.AttachmentId = image.Id;
        await _unitOfWork.CourseRepository.UpdateAsync(existingCourse);
        await _unitOfWork.SaveAsync();

        return _mapper.Map<CourseResultDto>(existingCourse);
    }
}