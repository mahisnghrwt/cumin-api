using AutoMapper;
using AutoMapper.Configuration.Conventions;
using cumin_api.Models;
using cumin_api.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cumin_api {
    public class AutomapperProfile: Profile {
        public AutomapperProfile() {
            AddMemberConfiguration().AddMember<NameSplitMember>();
            CreateMap<Epic, EpicDto>();
            CreateMap<IssueCreationDto, Issue>();
            CreateMap<SprintCreationDto, Sprint>();
            CreateMap<Project, ProjectBriefDto>();
            CreateMap<User, UserBriefDto>();
            CreateMap<Sprint, SprintBriefDto>();
            CreateMap<Epic, EpicBriefDto>();
            CreateMap<Issue, IssueDetailedDto>();
        }
    }
}