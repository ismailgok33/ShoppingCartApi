using CicekSepetiTask.Dtos;
using CicekSepetiTask.Entities;

namespace CicekSepetiTask.UnitTests
{
    public class AddItemTestParameters
    {
        public AddItemToCartDto TestItemDto { get; set; }
        public ShoppingCart TestShoppingCart { get; set; }
    }
}