using CicekSepetiTask.Base;
using CicekSepetiTask.Dtos;
using CicekSepetiTask.Entities;
using CicekSepetiTask.Services;
using CicekSepetiTask.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CicekSepetiTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartService _shoppingCartService;

        public ShoppingCartController(IShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetShoppingCart()
        {
            ShoppingCart cart = SessionHelper.GetShoppingCart(HttpContext.Session);
            return Ok(await _shoppingCartService.GetShoppingCart(cart));
        }

        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetSingleItem(Guid id)
        //{
        //    return Ok(await _shoppingCartService.GetItemById(id));
        //}

        [HttpPost]
        public async Task<IActionResult> AddItem(AddItemToCartDto item)
        {
            BaseResponse<int?> resp = new BaseResponse<int?>();
            ShoppingCart cart = SessionHelper.GetShoppingCart(HttpContext.Session);
            resp = await _shoppingCartService.AddItemToCart(item, cart);
            if (resp.Success)
            {
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
                return Ok(resp);
            }
            return Ok(resp);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(Guid id)
        {
            ShoppingCart cart = SessionHelper.GetShoppingCart(HttpContext.Session);
            BaseResponse<GetItemDto> response = await _shoppingCartService.RemoveItemFromCart(id, cart);
            if (response.ResponseCode == 404)
            {
                return NotFound(response);
            }
            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateItem(UpdateItemDto updatedItem)
        {
            ShoppingCart cart = SessionHelper.GetShoppingCart(HttpContext.Session);
            BaseResponse<int?> response = await _shoppingCartService.UpdateItemInCart(updatedItem, cart);
            if(response.ResponseCode == 404)
            {
                return NotFound(response);
            }
            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            return Ok(response);
        }
    }
}
