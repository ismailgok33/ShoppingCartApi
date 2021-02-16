using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CicekSepetiTask.Entities
{
    public class ShoppingCart : BaseEntity
    {
        public string UserId { get; set; }
        public float TotalPrice { get; set; }
        public int ItemCount { get; set; }
        public IList<ShoppingCartItem> ShoppingCartItems { get; set; }
    }
}
