using AutoMapper;
using Gym.Domain.Entities;
using Gym.Repository.UnitOfWorks;
using Gym.Service.DTOs.Courses;
using Gym.Service.Exceptions;
using Gym.Service.Interfaces;

namespace Gym.Service.Services;

public class CourseService : ICourseService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CourseService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
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
        var course = await _unitOfWork.CourseRepository.SelectAsync(q => q.Id == id);

        if (course is null)
            throw new NotFoundException("Course not found");

        return _mapper.Map<CourseResultDto>(course);
    }

    public async Task<IEnumerable<CourseResultDto>> GetAllAsync()
    {
        var courses = _unitOfWork.CourseRepository.SelectAll();
        return _mapper.Map<IEnumerable<CourseResultDto>>(courses);
    }

    public async Task<CourseResultDto> GetByIdWithVideosAsync(long id)
    {
        
        var course = await _unitOfWork.CourseRepository
            .SelectAsync(q => q.Id == id, includes: new string[] { "Videos" });

        if (course is null)
            throw new NotFoundException("Course not found");

        return _mapper.Map<CourseResultDto>(course);
    }

    public async Task<CourseResultDto> CreateAsync(CourseCreationDto dto)
    {
        var course = _mapper.Map<Course>(dto);
        await _unitOfWork.CourseRepository.AddAsync(course);
        await _unitOfWork.SaveAsync();
        
        return _mapper.Map<CourseResultDto>(course);
    }

    public async Task<CourseResultDto> UpdateAsync(CourseUpdateDto dto)
    {
        var existingCourse = await _unitOfWork.CourseRepository.SelectAsync(q => q.Id == dto.Id);
        if (existingCourse is null)
            throw new NotFoundException("Course not found");

        _mapper.Map(dto, existingCourse);
        await _unitOfWork.CourseRepository.UpdateAsync(existingCourse);
        await _unitOfWork.SaveAsync();

        return _mapper.Map<CourseResultDto>(existingCourse);
    }
}