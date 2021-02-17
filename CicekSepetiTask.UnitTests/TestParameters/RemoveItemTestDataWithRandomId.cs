using CicekSepetiTask.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CicekSepetiTask.UnitTests
{
    class RemoveItemTestDataWithRandomId : TheoryData<RemoveItemTestParameters>
    {
        public RemoveItemTestDataWithRandomId()
        {
            Add(new RemoveItemTestParameters
            {
                Id = Guid.NewGuid(),
                TestShoppingCart = new ShoppingCart
                {
                    Id = Guid.NewGuid(),
                    UserId = "234234-6456-234234",
                    TotalPrice = 23.21f,
                    ItemCount = 3,
                    ShoppingCartItems = null
                }
            });
        }
    }
}
