using JobsServices.models.Dto;
using JobsServices.models.Dto;
using JobsServices.models.Entity;
using AutoMapper;

namespace JobsServices
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            // Map between Job and JobDto
            CreateMap<Job, JobDto>().ReverseMap();

            // Map between CreateJobDto and Job
            CreateMap<CreateJobDto, Job>();
        }
    }
}

