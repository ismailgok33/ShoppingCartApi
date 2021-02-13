using CicekSepetiTask.Entities;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CicekSepetiTask.Utility
{
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

        public static ShoppingCart GetShoppingCart(ISession session)
        {
            if (GetObjectFromJson<ShoppingCart>(session, "cart") == null)
            {
                return new ShoppingCart()
                {
                    Id = Guid.NewGuid(),
                    ShoppingCartItems = new List<ShoppingCartItem>()
                };
            }
            else
            {
                return GetObjectFromJson<ShoppingCart>(session, "cart");
            }
        }
    }
}
