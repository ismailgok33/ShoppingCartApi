using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CicekSepetiTask.Dtos
{
    public class UpdateItemDto
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; }
    }
}
