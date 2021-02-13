using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CicekSepetiTask.Entities
{
    public class ShoppingCartItem : BaseEntity
    {
        public Item Item { get; set; }
        public int Quantity { get; set; }
        public Guid ShoppingCartId { get; set; }
    }
}
