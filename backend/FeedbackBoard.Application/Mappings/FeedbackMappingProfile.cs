using AutoMapper;
using FeedbackBoard.Application.DTOs;
using FeedbackBoard.Domain.Entities;

namespace FeedbackBoard.Application.Mappings
{
    /// <summary>
    /// AutoMapper profile for mapping between entities and DTOs
    /// </summary>
    public class FeedbackMappingProfile : Profile
    {
        public FeedbackMappingProfile()
        {
            // Entity to DTO mappings
            CreateMap<Feedback, FeedbackDto>();

            // DTO to Entity mappings
            CreateMap<CreateFeedbackDto, Feedback>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

            CreateMap<FeedbackDto, Feedback>()
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
        }
    }
}
