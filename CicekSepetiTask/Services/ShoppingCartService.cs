using AutoMapper;
using CicekSepetiTask.Base;
using CicekSepetiTask.Dtos;
using CicekSepetiTask.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CicekSepetiTask.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        //private IShoppingCartService _shoppingCart;

        private static IList<Item> _itemsInCard = new List<Item>
        {
            new Item {Id = Guid.NewGuid(), Name = "testItem"},
            new Item {Id = Guid.NewGuid(), Name = "testItem2"}
        };
        private readonly IMapper _mapper;

        public ShoppingCartService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<BaseResponse<IList<GetItemDto>>> AddItemToCart(AddItemToCartDto item)
        {
            BaseResponse<IList<GetItemDto>> response = new BaseResponse<IList<GetItemDto>>();
            _itemsInCard.Add(_mapper.Map<Item>(item));
            response.Data = _itemsInCard.Select(item => _mapper.Map<GetItemDto>(item)).ToList();
            return response;
        }

        public async Task<BaseResponse<IList<GetItemDto>>> GetAllItems()
        {
            BaseResponse<IList<GetItemDto>> response = new BaseResponse<IList<GetItemDto>>();
            response.Data = _itemsInCard.Select(item => _mapper.Map<GetItemDto>(item)).ToList();
            return response;
        }

        public async Task<BaseResponse<GetItemDto>> GetItemById(Guid id)
        {
            BaseResponse<GetItemDto> response = new BaseResponse<GetItemDto>();
            response.Data = _mapper.Map<GetItemDto>(_itemsInCard.FirstOrDefault(item => item.Id == id));
            return response;
        }

        public async Task<BaseResponse<IList<GetItemDto>>> RemoveItemFromCart(Guid id)
        {
            BaseResponse<IList<GetItemDto>> response = new BaseResponse<IList<GetItemDto>>();

            try
            {
                Item item = _itemsInCard.First(i => i.Id == id);
                _itemsInCard.Remove(item);

                response.Data = _itemsInCard.Select(item => _mapper.Map<GetItemDto>(item)).ToList();
            }
            catch (Exception e)
            {
                response.Success = false;
                response.ResponseCode = 404;
                response.ErrorMessage = e.Message;
            }
            return response;
        }

        public async Task<BaseResponse<GetItemDto>> UpdateItemInCart(UpdateItemDto updatedItemDto)
        {
            BaseResponse<GetItemDto> response = new BaseResponse<GetItemDto>();

            try
            {
                Item item = _itemsInCard.FirstOrDefault(i => i.Id == updatedItemDto.Id);
                item.Name = updatedItemDto.Name;
                item.Price = updatedItemDto.Price;
                item.Stock = updatedItemDto.Stock;

                response.Data = _mapper.Map<GetItemDto>(item);
            } 
            catch(Exception e)
            {
                response.Success = false;
                response.ResponseCode = 404;
                response.ErrorMessage = e.Message;
            }
            return response;
        }
    }
}
