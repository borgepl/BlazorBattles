using AutoMapper;
using DataAccess.Data.Domain;
using BlazorBattles.Models.Dto;

namespace BlazorBattles.Server.Config
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<UserUnit, UserUnitResponseDTO>();
           
        }
    }
}