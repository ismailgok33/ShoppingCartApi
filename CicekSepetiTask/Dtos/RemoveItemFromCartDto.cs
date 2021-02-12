using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CicekSepetiTask.Dtos
{
    public class RemoveItemFromCartDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public int Count { get; set; }
    }
}
