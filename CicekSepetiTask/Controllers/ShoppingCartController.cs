using CicekSepetiTask.Base;
using CicekSepetiTask.Dtos;
using CicekSepetiTask.Entities;
using CicekSepetiTask.Services;
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
        public async Task<IActionResult> GetAllItems()
        {
            return Ok(await _shoppingCartService.GetAllItems());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSingleItem(Guid id)
        {
            return Ok(await _shoppingCartService.GetItemById(id));
        }

        [HttpPost]
        public async Task<IActionResult> AddItem(AddItemToCartDto item)
        {
            return Ok(await _shoppingCartService.AddItemToCart(item));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(Guid id)
        {
            BaseResponse<IList<GetItemDto>> response = await _shoppingCartService.RemoveItemFromCart(id);
            if (response.ResponseCode == 404)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateItem(UpdateItemDto updatedItem)
        {
            BaseResponse<GetItemDto> response = await _shoppingCartService.UpdateItemInCart(updatedItem);
            if(response.ResponseCode == 404)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
    }
}
