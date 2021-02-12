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
            CreateMap<AddItemToCartDto, Item>();
        }
    }
}
