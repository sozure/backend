using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VGManager.Repository.Entities;
using VGManager.Services.Models.Projects;

namespace VGManager.Services.MapperProfiles;
public class ProjectProfile : Profile
{
    public ProjectProfile()
    {
        CreateMap<ProjectEntity, ProjectResultModel>();
    }
}
