using AutoMapper;
using CicekSepetiTask.Dtos;
using CicekSepetiTask.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CicekSepetiTask
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Item, GetItemDto>();
            CreateMap<ShoppingCartItem, GetItemDto>()
                .ForMember(
                dest => dest.Name,
                opt => opt.MapFrom(src => src.Item.Name)
                )
                .ForMember(
                 dest => dest.Price,
                opt => opt.MapFrom(src => src.Item.Price)
                );
            CreateMap<AddItemToCartDto, Item>();
        }
    }
}
