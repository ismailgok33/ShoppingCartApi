using AutoMapper;
using CicekSepetiTask.Base;
using CicekSepetiTask.Repositories;
using CicekSepetiTask.Dtos;
using CicekSepetiTask.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CicekSepetiTask.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        public ShoppingCartService(IMapper mapper, DataContext context)
        {
            //repo
            _mapper = mapper;
            _context = context;
        }

        public async Task<BaseResponse<int?>> AddItemToCart(AddItemToCartDto itemDto, ShoppingCart shoppingCart)
        {
            Item dbItem = await _context.Items.FirstOrDefaultAsync(i => i.Id == itemDto.Id);
            
            ShoppingCartItem sci = shoppingCart.ShoppingCartItems.FirstOrDefault(i => i.Item.Id == dbItem.Id);
            
            if(sci != null)
            {
                sci.Quantity += itemDto.Quantity;
                if(!isInStock(dbItem, sci))
                {
                    //throw new ShoppingCartException("Not enough stock!", 500);
                    throw new Exception("Not enough stock!");
                }
                //_context.ShoppingCartItems.Update(sci);
            }
            else
            {
                sci = new ShoppingCartItem()
                {
                    Id = Guid.NewGuid(),
                    Item = dbItem,
                    Quantity = itemDto.Quantity,
                    ShoppingCartId = shoppingCart.Id
                };

                if (!isInStock(dbItem, sci))
                {
                    //throw new ShoppingCartException("Not enough stock!", 500);
                    throw new Exception("Not enough stock!");
                }
                //await _context.ShoppingCartItems.AddAsync(sci);
                shoppingCart.ShoppingCartItems.Add(sci);
                // TODO item count çıkabilir.
                shoppingCart.ItemCount += 1;
            }
            shoppingCart.TotalPrice = shoppingCart.ShoppingCartItems.Sum(item => item.Quantity * item.Item.Price);

            //await _context.SaveChangesAsync();
            return new BaseResponse<int?> ();
        }

        public async Task<BaseResponse<ShoppingCart>> GetShoppingCart(ShoppingCart shoppingCart)
        {
            BaseResponse<ShoppingCart> response = new BaseResponse<ShoppingCart>();
            //IList<ShoppingCartItem> dbItems = shoppingCart.ShoppingCartItems;
            response.Data = shoppingCart;
            return response;
        }

        /*public async Task<BaseResponse<GetItemDto>> GetItemById(Guid id)
        {
            BaseResponse<GetItemDto> response = new BaseResponse<GetItemDto>();
            Item dbItem = await _context.Items.FirstOrDefaultAsync(item => item.Id == id);
            response.Data = _mapper.Map<GetItemDto>(dbItem);
            return response;
        }*/

        public async Task<BaseResponse<GetItemDto>> RemoveItemFromCart(Guid id, ShoppingCart shoppingCart)
        {
            BaseResponse<GetItemDto> response = new BaseResponse<GetItemDto>();

            try
            {
                ShoppingCartItem sci = shoppingCart.ShoppingCartItems.First(item => item.Id == id);
                response.Data = _mapper.Map<GetItemDto>(sci);
                shoppingCart.ShoppingCartItems.Remove(sci);
                shoppingCart.ItemCount--;
                shoppingCart.TotalPrice -= sci.Item.Price * sci.Quantity;
            }
            catch (Exception e)
            {
                response.Success = false;
                response.ResponseCode = 404;
                response.ErrorMessage = e.Message;
            }
            return response;
        }

        public async Task<BaseResponse<int?>> UpdateItemInCart(UpdateItemDto updatedItemDto, ShoppingCart shoppingCart)
        {
            BaseResponse<int?> response = new BaseResponse<int?>();

            try
            {
                ShoppingCartItem sci = shoppingCart.ShoppingCartItems.FirstOrDefault(i => i.Id == updatedItemDto.Id);

                Item dbItem = await _context.Items.FirstOrDefaultAsync(i => i.Id == sci.Item.Id);
                if (dbItem.Stock < updatedItemDto.Quantity)
                {
                    //throw new ShoppingCartException("Not enough stock!", 500);
                    throw new Exception("Not enough stock!");
                }

                sci.Quantity = updatedItemDto.Quantity;

                //_context.Items.Update(dbItem);
                //await _context.SaveChangesAsync();
            } 
            catch(Exception e)
            {
                response.Success = false;
                response.ResponseCode = 404;
                response.ErrorMessage = e.Message;
            }
            return response;
        }

        private bool isInStock(Item item, ShoppingCartItem sci)
        {
            if (item.Stock < sci.Quantity)
            {
                return false;
                
            }
            return true;
        }
    }
}
