using CicekSepetiTask.Entities;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CicekSepetiTask.Utility
{
    /// <summary>
    /// This class helps keeping Shopping Cart instance in the Session and then persist it to the Redis
    /// </summary>
    public static class SessionHelper
    {
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }

        public static ShoppingCart GetShoppingCart(ISession session, string userId)
        {
            if (GetObjectFromJson<ShoppingCart>(session, userId) == null)
            {
                return new ShoppingCart()
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    ShoppingCartItems = new List<ShoppingCartItem>()
                };
            }
            else
            {
                return GetObjectFromJson<ShoppingCart>(session, userId);
            }
        }

        /// <summary>
        /// This is used when an anonymous user want to add an Item to the Shopping Cart.
        /// If the user does not have a session in the cookie, then a newly generated Id is set to the session.
        /// Otherwise the Id is retrived.
        /// </summary>
        /// <param name="response"> Http Response </param>
        /// <param name="key"> Session key </param>
        /// <param name="value"> Session value (User Id) </param>
        /// <param name="expireTime"> Cookie expire time </param>
        public static void Set(HttpResponse response, string key, string value, int? expireTime)
        {
            CookieOptions option = new CookieOptions();

            if (expireTime.HasValue)
                option.Expires = DateTime.Now.AddDays(expireTime.Value);
            else
                option.Expires = DateTime.Now.AddMilliseconds(10);

            response.Cookies.Append(key, value, option);
        }
    }
}
