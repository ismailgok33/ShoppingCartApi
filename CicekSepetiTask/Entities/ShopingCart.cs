using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CicekSepetiTask.Entities
{
    public class ShopingCart : BaseEntity
    {
        public int UserId { get; set; }
        public float TotalPrice { get; set; }
        public int ItemCount { get; set; }
        public IList<Item> Items { get; set; }
    }
}
