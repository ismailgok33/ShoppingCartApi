using AutoMapper;
using CicekSepetiTask.Exceptions;
using CicekSepetiTask.Repositories;
using CicekSepetiTask.Services;
using System.Threading.Tasks;
using Xunit;

namespace CicekSepetiTask.UnitTests
{
    public class ShoppingCartServiceTests
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public ShoppingCartServiceTests(IMapper mapper, DataContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        [Theory(DisplayName = "Add Item To Cart")]
        [ClassData(typeof(AddItemTestDataWithRandomId))]
        public async Task AddItemToCart_ShouldItemNotFound_WhenIdIsNotInItemTableAsync(AddItemTestParameters parameters)
        {
            ShoppingCartService service = new ShoppingCartService(_mapper, _context);
            var result = await Record.ExceptionAsync(() => service.AddItemToCart(parameters.TestItemDto, parameters.TestShoppingCart));
            var exception = Assert.IsType<ItemNotFoundException>(result);
        }

        [Theory(DisplayName = "Add Item To Cart")]
        [ClassData(typeof(RemoveItemTestDataWithRandomId))]
        public async Task RemoveItemFromCart_ShouldItemNotFound_WhenIdIsNotInItemTableAsync(RemoveItemTestParameters parameters)
        {
            ShoppingCartService service = new ShoppingCartService(_mapper, _context);
            var result = await Record.ExceptionAsync(() => service.RemoveItemFromCart(parameters.Id, parameters.TestShoppingCart));
            var exception = Assert.IsType<ItemNotFoundException>(result);
        }

        [Theory(DisplayName = "Add Item To Cart")]
        [ClassData(typeof(UpdateItemTestDataWithRandomId))]
        public async Task UpdateItemFromCart_ShouldItemNotFound_WhenIdIsNotInItemTableAsync(UpdateItemTestParameters parameters)
        {
            ShoppingCartService service = new ShoppingCartService(_mapper, _context);
            var result = await Record.ExceptionAsync(() => service.UpdateItemInCart(parameters.TestItemDto, parameters.TestShoppingCart));
            var exception = Assert.IsType<ItemNotFoundException>(result);
        }

    }
}