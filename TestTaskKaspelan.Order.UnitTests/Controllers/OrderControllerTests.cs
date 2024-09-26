using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using TestTaskKaspelan.Order.Controllers;
using TestTaskKaspelan.Order.Services.Interfaces;
using ContractOrder = TestTaskKaspelan.Common.Contracts.Order;

namespace TestTaskKaspelan.Order.UnitTests.Controllers
{
    public class OrderControllerTests
    {
        Fixture _fixture;
        Mock<IOrderService> _orderService;
        Mock<ILogger<OrderController>> _logger;
        OrderController _orderController;

        public OrderControllerTests()
        {
            _fixture = new Fixture();
            _orderService = new Mock<IOrderService>();
            _logger = new Mock<ILogger<OrderController>>();
            _orderController = new OrderController(_orderService.Object, _logger.Object);
        }

        [Fact]
        public async void OrderController_Get_ReturnArgumentException()
        {
            var orderId = Guid.Empty;
            var token = _fixture.Create<CancellationToken>();

            var exceptionResult = await Assert.ThrowsAsync<ArgumentException>(async () => await _orderController.Get(orderId, token));
            _orderService.Verify(service => service.GetAsync(orderId, token), Times.Never);

            Assert.NotNull(exceptionResult);
            Assert.IsType<ArgumentException>(exceptionResult);
            Assert.Equal(nameof(orderId), exceptionResult.Message);
        }

        [Fact]
        public async void OrderController_Get_ReturnUnexpectedException()
        {
            var exception = _fixture.Create<Exception>();
            var orderId = _fixture.Create<Guid>();
            var token = _fixture.Create<CancellationToken>();

            _orderService.Setup(service => service.GetAsync(orderId, token)).ThrowsAsync(exception);
            var exceptionResult = await Assert.ThrowsAsync<Exception>(async () => await _orderController.Get(orderId, token));
            _orderService.Verify(service => service.GetAsync(orderId, token), Times.Once);

            Assert.NotNull(exceptionResult);
            Assert.IsType<Exception>(exceptionResult);
            Assert.Equal(exception, exceptionResult);
        }

        [Fact]
        public async void OrderController_Get_ReturnValidResult()
        {
            var orderId = _fixture.Create<Guid>();
            var orderResult = _fixture.Create<ContractOrder>();
            var token = _fixture.Create<CancellationToken>();

            _orderService.Setup(service => service.GetAsync(orderId, token)).ReturnsAsync(orderResult);
            var result = await _orderController.Get(orderId, token);
            _orderService.Verify(service => service.GetAsync(orderId, token), Times.Once);

            Assert.NotNull(result);
            Assert.IsType<ContractOrder>(result);
            Assert.Equal(orderResult.Id, result.Id);
            Assert.Equal(orderResult.Details, result.Details);
        }

        [Fact]
        public async void OrderController_Create_ReturnUnexpectedException()
        {
            var exception = _fixture.Create<Exception>();
            var order = _fixture.Create<ContractOrder>();
            var token = _fixture.Create<CancellationToken>();

            _orderService.Setup(service => service.CreateAsync(order, token)).ThrowsAsync(exception);
            var exceptionResult = await Assert.ThrowsAsync<Exception>(async () => await _orderController.Create(order, token));
            _orderService.Verify(service => service.CreateAsync(order, token), Times.Once);

            Assert.NotNull(exceptionResult);
            Assert.IsType<Exception>(exceptionResult);
            Assert.Equal(exception, exceptionResult);
        }

        [Fact]
        public async void OrderController_Create_ReturnValidResult()
        {
            var order = _fixture.Create<ContractOrder>();
            var token = _fixture.Create<CancellationToken>();

            _orderService.Setup(service => service.CreateAsync(order, token)).ReturnsAsync(order);
            var result = await _orderController.Create(order, token);
            _orderService.Verify(service => service.CreateAsync(order, token), Times.Once);

            Assert.NotNull(result);
            Assert.IsType<ContractOrder>(result);
            Assert.Equal(order, result);
        }
    }
}