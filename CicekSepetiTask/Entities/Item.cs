using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CicekSepetiTask.Entities
{
    public class Item : BaseEntity
    {
        //public Guid Id { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public int Stock { get; set; }
    }
}
