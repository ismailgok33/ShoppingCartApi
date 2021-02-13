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
        Task<BaseResponse<ShoppingCart>> GetShoppingCart(ShoppingCart shoppingCart);
        //Task<BaseResponse<GetItemDto>> GetItemById(Guid id);
        Task<BaseResponse<int?>> AddItemToCart(AddItemToCartDto item, ShoppingCart shoppingCart);
        Task<BaseResponse<GetItemDto>> RemoveItemFromCart(Guid id, ShoppingCart shoppingCart);
        Task<BaseResponse<int?>> UpdateItemInCart(UpdateItemDto updatedItemDto, ShoppingCart shoppingCart);
    }
}
