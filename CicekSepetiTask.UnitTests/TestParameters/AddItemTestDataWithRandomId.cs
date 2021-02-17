using CicekSepetiTask.Dtos;
using CicekSepetiTask.Entities;
using System;
using Xunit;

namespace CicekSepetiTask.UnitTests
{
    public class AddItemTestDataWithRandomId : TheoryData<AddItemTestParameters>
    {
        public AddItemTestDataWithRandomId()
        {
            Add(new AddItemTestParameters
            {
                TestItemDto = new AddItemToCartDto { Id = Guid.NewGuid(), Quantity = 5 },
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