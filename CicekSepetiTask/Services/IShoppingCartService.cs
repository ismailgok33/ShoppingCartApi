using CicekSepetiTask.Base;
using CicekSepetiTask.Dtos;
using CicekSepetiTask.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CicekSepetiTask.Services
{
    public interface IShoppingCartService
    {
        Task<BaseResponse<IList<GetItemDto>>> GetAllItems();
        Task<BaseResponse<GetItemDto>> GetItemById(Guid id);
        Task<BaseResponse<IList<GetItemDto>>> AddItemToCart(AddItemToCartDto item);
        Task<BaseResponse<IList<GetItemDto>>> RemoveItemFromCart(Guid id);
        Task<BaseResponse<GetItemDto>> UpdateItemInCart(UpdateItemDto updatedItemDto);
    }
}
