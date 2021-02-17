using CicekSepetiTask.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CicekSepetiTask.UnitTests
{
    public class RemoveItemTestParameters
    {
        public Guid Id { get; set; }
        public ShoppingCart TestShoppingCart { get; set; }
    }
}
