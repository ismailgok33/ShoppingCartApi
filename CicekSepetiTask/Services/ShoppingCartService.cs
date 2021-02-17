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
using CicekSepetiTask.Utility;
using CicekSepetiTask.Exceptions;

namespace CicekSepetiTask.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public ShoppingCartService(IMapper mapper, DataContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        /// <summary>
        /// This method add an item to the Shopping Cart.
        /// The item must already be in the Item Table (a.k.a the inventory)
        /// AddItemToCartDto has two properties: ItemId and Quantity.
        /// ItemId must match the Id of the item from the Item Table (Inventory)
        /// (Note: Items are created via SQL commands when the application first runs)
        /// Throws a generic exception to be handled by the CustomExceptionFilter (HttpGlobalExceptionFilter)
        /// </summary>
        /// <param name="itemDto"> ItemDto to be added to the Shopping Cart</param>
        /// <param name="shoppingCart"> Shopping Cart Entity </param>
        /// <returns> Returns a simple response </returns>
        public async Task<BaseResponse<int?>> AddItemToCart(AddItemToCartDto itemDto, ShoppingCart shoppingCart)
        {
            Item dbItem = await _context.Items.FirstOrDefaultAsync(i => i.Id == itemDto.Id);

            if (dbItem == null)
            {
                throw new ItemNotFoundException();
            }

            ShoppingCartItem shoppingCartItem = shoppingCart.ShoppingCartItems.FirstOrDefault(i => i.Item.Id == dbItem.Id);

            if (shoppingCartItem != null)
            {
                shoppingCartItem.Quantity += itemDto.Quantity;
                if (!IsInStock(dbItem, shoppingCartItem))
                {
                    throw new NotEnoughStockException();
                }
            }
            else
            {
                shoppingCartItem = new ShoppingCartItem()
                {
                    Id = Guid.NewGuid(),
                    Item = dbItem,
                    Quantity = itemDto.Quantity,
                    ShoppingCartId = shoppingCart.Id
                };

                if (!IsInStock(dbItem, shoppingCartItem))
                {
                    throw new NotEnoughStockException();
                }
                shoppingCart.ShoppingCartItems.Add(shoppingCartItem);
                shoppingCart.ItemCount += 1;
            }
            shoppingCart.TotalPrice = shoppingCart.ShoppingCartItems.Sum(item => item.Quantity * item.Item.Price);

            return new BaseResponse<int?>();
        }

        /// <summary>
        /// If a user is logged in returns the items in the shopping cart of that user. 
        /// The Shopping Cart is stored in the Redis Server
        /// If a user is not logged in, returns the items in the shopping cart of session holder.
        /// </summary>
        /// <param name="shoppingCart"> The Shopping Cart to be returned </param>
        /// <returns> All Items in the Shopping Cart as response </returns>
        public BaseResponse<ShoppingCart> GetShoppingCart(ShoppingCart shoppingCart)
        {
            BaseResponse<ShoppingCart> response = new BaseResponse<ShoppingCart>
            {
                Data = shoppingCart
            };
            return response;
        }

        /// <summary>
        /// The method gets the Id (Shopping Cart ItemId, not ItemId from the Item Table) of the Item in the Shopping Cart
        /// and deletes the selected item from the Shopping Cart.
        /// </summary>
        /// <param name="id"> The Shopping Cart Id of the Item in the Shopping Cart</param>
        /// <param name="shoppingCart"> The Shopping Cart </param>
        /// <returns> Returns the deleted item as response </returns>
        public async Task<BaseResponse<GetItemDto>> RemoveItemFromCart(Guid id, ShoppingCart shoppingCart)
        {
            BaseResponse<GetItemDto> response = new BaseResponse<GetItemDto>();

            try
            {
                ShoppingCartItem shoppingCartItem = shoppingCart.ShoppingCartItems.First(item => item.Id == id);
                response.Data = _mapper.Map<GetItemDto>(shoppingCartItem);
                shoppingCart.ShoppingCartItems.Remove(shoppingCartItem);
                shoppingCart.ItemCount--;
                shoppingCart.TotalPrice -= shoppingCartItem.Item.Price * shoppingCartItem.Quantity;
            }
            catch (Exception)
            {
                throw new ItemNotFoundException();
            }
            return response;
        }

        /// <summary>
        /// Gets an Item as UpdateItemDto and replaces the quantity of that Item in the Shopping Cart.
        /// </summary>
        /// <param name="updatedItemDto"> The Item that you want to update its quantity </param>
        /// <param name="shoppingCart"> The Shopping Cart </param>
        /// <returns> Returns the updated item as response </returns>
        public async Task<BaseResponse<int?>> UpdateItemInCart(UpdateItemDto updatedItemDto, ShoppingCart shoppingCart)
        {
            BaseResponse<int?> response = new BaseResponse<int?>();

            try
            {
                ShoppingCartItem shoppingCartItem = shoppingCart.ShoppingCartItems.FirstOrDefault(i => i.Id == updatedItemDto.Id);

                Item dbItem = await _context.Items.FirstOrDefaultAsync(i => i.Id == shoppingCartItem.Item.Id);
                if (dbItem.Stock < updatedItemDto.Quantity)
                {
                    throw new NotEnoughStockException();
                }
                shoppingCartItem.Quantity = updatedItemDto.Quantity;
            }
            catch (Exception)
            {
                throw new ItemNotFoundException();
            }
            return response;
        }

        /// <summary>
        /// Simple mathematical operation to determine an Item has enough stock
        /// Created a seperate method to obey SOLID rules.
        /// </summary>
        /// <param name="item"> Item from Item Table (Inventory) to be added or removed </param>
        /// <param name="sci"> Shopping Cart Item to match the stock of the Inventory </param>
        /// <returns> True or false </returns>
        private bool IsInStock(Item item, ShoppingCartItem sci)
        {
            if (item.Stock < sci.Quantity)
            {
                return false;

            }
            return true;
        }
    }
}
