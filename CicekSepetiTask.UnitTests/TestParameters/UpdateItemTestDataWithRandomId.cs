using CicekSepetiTask.Dtos;
using CicekSepetiTask.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CicekSepetiTask.UnitTests
{
    public class UpdateItemTestDataWithRandomId : TheoryData<UpdateItemTestParameters>
    {
        public UpdateItemTestDataWithRandomId()
        {
            Add(new UpdateItemTestParameters
            {
                TestItemDto = new UpdateItemDto { Id = Guid.NewGuid(), Quantity = 5 },
                TestShoppingCart = new ShoppingCart
                {
                    Id = Guid.NewGuid(),
                    UserId = "2342-25235-1243124",
                    TotalPrice = 23,
                    ItemCount = 1,
                    ShoppingCartItems = null
                }
            });
        }
    }
}
