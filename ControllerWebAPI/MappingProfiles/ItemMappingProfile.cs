using AutoMapper;
using ControllerWebAPI.Models;
using ControllerWebAPI.Models.APIModels;

namespace ControllerWebAPI.MappingProfiles;

public class ItemMappingProfile : Profile
{
    public ItemMappingProfile()
    {
        CreateMap<Item, ItemAPIModel>().ReverseMap();
    }
}