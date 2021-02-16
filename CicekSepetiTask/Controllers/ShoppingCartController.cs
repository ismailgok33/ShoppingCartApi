using CicekSepetiTask.Base;
using CicekSepetiTask.Dtos;
using CicekSepetiTask.Entities;
using CicekSepetiTask.Services;
using CicekSepetiTask.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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

        /// <summary>
        /// If a user is logged in returns the items in the shopping cart of that user.
        /// If a user is not logged in, returns the items in the shopping cart of session holder.
        /// </summary>
        /// <returns> Returns all items currently in the shopping cart</returns>
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetShoppingCart()
        {
            var userId = GetId();
            ShoppingCart cart = SessionHelper.GetShoppingCart(HttpContext.Session, userId);
            return Ok(_shoppingCartService.GetShoppingCart(cart));
        }

        /// <summary>
        /// This method add an item to the Shopping Cart.
        /// The item must already be in the Item Table (a.k.a the inventory)
        /// AddItemToCartDto has two properties: ItemId and Quantity.
        /// ItemId must match the Id of the item from the Item Table (Inventory)
        /// (Note: Items are created via SQL commands when the application first runs)
        /// </summary>
        /// <param name="item"> AddItemToCartDto that has two properties: ItemId and Quantity. </param>
        /// <returns> Returns a simple response </returns>
        [HttpPost]
        public async Task<IActionResult> AddItem(AddItemToCartDto item)
        {
            BaseResponse<int?> resp = new BaseResponse<int?>();
            var userId = GetId();
            ShoppingCart cart = SessionHelper.GetShoppingCart(HttpContext.Session, userId);
            resp = await _shoppingCartService.AddItemToCart(item, cart);
            if (resp.Success)
            {
                SessionHelper.SetObjectAsJson(HttpContext.Session, userId, cart);
                return Ok(resp);
            }
            return Ok(resp);
        }

        /// <summary>
        /// The method gets the Id (Shopping Cart ItemId, not ItemId from the Item Table) of the Item in the Shopping Cart
        /// and deletes the selected item from the Shopping Cart.
        /// </summary>
        /// <param name="id"> The Shopping Cart Id of the Item in the Shopping Cart </param>
        /// <returns> Returns the deleted item as response </returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(Guid id)
        {
            var userId = GetId();
            ShoppingCart cart = SessionHelper.GetShoppingCart(HttpContext.Session, userId);
            BaseResponse<GetItemDto> response = await _shoppingCartService.RemoveItemFromCart(id, cart);
            if (response.ResponseCode == 404)
            {
                return NotFound(response);
            }
            SessionHelper.SetObjectAsJson(HttpContext.Session, userId, cart);
            return Ok(response);
        }

        /// <summary>
        /// Gets an Item as UpdateItemDto and replaces the quantity of that Item in the Shopping Cart.
        /// </summary>
        /// <param name="updatedItem"> The Item that you want to update its quantity </param>
        /// <returns> Returns the updated item as response </returns>
        [HttpPut]
        public async Task<IActionResult> UpdateItem(UpdateItemDto updatedItem)
        {
            var userId = GetId();
            ShoppingCart cart = SessionHelper.GetShoppingCart(HttpContext.Session, userId);
            BaseResponse<int?> response = await _shoppingCartService.UpdateItemInCart(updatedItem, cart);
            if (response.ResponseCode == 404)
            {
                return NotFound(response);
            }
            SessionHelper.SetObjectAsJson(HttpContext.Session, userId, cart);
            return Ok(response);
        }

        /// <summary>
        /// The method provides userId for the shopping cards.
        /// If a user is logged in, then userId is retrived from the Redis by username of the user's JWT token claim.
        /// If a user is not logged in, then userId is retrived from the session of the anonymous user.
        /// If the session is newly created and/or is not assigned to a cookie, new userId is returned and set to session.
        /// </summary>
        /// <returns> return UserId for the purpose of identifying the Shopping Cart</returns>
        private string GetId()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            string userId = Guid.NewGuid().ToString();
            if (identity != null && identity.Claims != null && identity.Claims.Count() > 0)
            {
                IEnumerable<Claim> claims = identity.Claims;
                userId = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value.ToString();
            }
            else
            {
                string sid = Request.Cookies["sid"];
                if (sid != null)
                {
                    userId = sid;
                }
                SessionHelper.Set(Response, "sid", userId, 1);
            }
            return userId;
        }
    }
}
