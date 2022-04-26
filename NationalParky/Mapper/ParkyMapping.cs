using AutoMapper;
using NationalParky.Models;
using NationalParky.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NationalParky.Mapper
{
    public class ParkyMapping:Profile
    {
        public ParkyMapping()
        {
            CreateMap<RegisterModel, ApplicationUser>().ReverseMap();
            CreateMap<NationalPark, NationalParkDto>().ReverseMap();
            CreateMap<Trail, TrailDto>().ReverseMap();
            CreateMap<Trail, TrailCreateDto>().ReverseMap();
            CreateMap<Trail, TrailUpdateDto>().ReverseMap();

        }
    }
}
