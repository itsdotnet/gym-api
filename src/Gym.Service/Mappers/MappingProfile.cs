using AutoMapper;
using Gym.Domain.Entities;
using Gym.Service.DTOs.Attachments;
using Gym.Service.DTOs.Courses;
using Gym.Service.DTOs.Images;
using Gym.Service.DTOs.Users;
using Gym.Service.DTOs.Videos;

namespace Gym.Service.Mappers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserResultDto>().ReverseMap();
        CreateMap<UserUpdateDto, User>().ReverseMap();
        CreateMap<UserCreationDto, User>().ReverseMap();
        
        CreateMap<Course,CourseCreationDto>().ReverseMap();
        CreateMap<Course, CourseUpdateDto>().ReverseMap();
        CreateMap<CourseResultDto, Course>().ReverseMap();
        
        CreateMap<Image,ImageCreationDto>().ReverseMap();
        CreateMap<Image, ImageUpdateDto>().ReverseMap();
        CreateMap<ImageResultDto, Image>().ReverseMap();
        
        CreateMap<Video,VideoCreationDto>().ReverseMap();
        CreateMap<Video, VideoUpdateDto>().ReverseMap();
        CreateMap<VideoResultDto, Video>().ReverseMap();
        
        CreateMap<Attachment, AttachmentResultDto>().ReverseMap();
    }
}