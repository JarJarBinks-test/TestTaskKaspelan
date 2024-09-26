using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using Steeltoe.Messaging.RabbitMQ.Core;
using TestTaskKaspelan.Order.Services.Services;
using TestTaskKaspelan.Order.Services.Interfaces;
using TestTaskKaspelan.Order.Services.Exceptions;
using TestTaskKaspelan.Order.DataAccess.Interfaces;
using DbOrder = TestTaskKaspelan.Order.DataAccess.Entities.Order;
using ContractOrder = TestTaskKaspelan.Common.Contracts.Order;
using TestTaskKaspelan.Common.Constants;
using Steeltoe.Messaging;

namespace TestTaskKaspelan.Order.Services.UnitTests.Services
{
    public class OrderServiceTests
    {
        Fixture _fixture;
        Mock<IOrderRepository> _orderRepository;
        Mock<IRabbitTemplate> _rabbitTemplate;
        Mock<ILogger<OrderService>> _logger;
        IOrderService _orderService;

        public OrderServiceTests()
        {
            _fixture = new Fixture();

            _orderRepository = new Mock<IOrderRepository> ();
            _rabbitTemplate = new Mock<IRabbitTemplate>();
            _logger = new Mock<ILogger<OrderService>>();
            _orderService = new OrderService(_orderRepository.Object, _rabbitTemplate.Object, _logger.Object);
        }

        [Fact]
        public async void OrderService_GetAsync_ThrowArgumentException()
        {
            var orderId = Guid.Empty;
            var token = _fixture.Create<CancellationToken>();

            var exceptionResult = await Assert.ThrowsAsync<ArgumentException>(async () => await _orderService.GetAsync(orderId, token));
            _orderRepository.Verify(repo => repo.GetAsync(orderId, token), Times.Never);

            Assert.NotNull(exceptionResult);
            Assert.IsType<ArgumentException>(exceptionResult);
            Assert.Equal(nameof(orderId), exceptionResult.Message);
        }

        [Fact]
        public async void OrderService_GetAsync_ThrowNotFoundException()
        {
            var orderId = _fixture.Create<Guid>();
            var token = _fixture.Create<CancellationToken>();

            _orderRepository.Setup(repo => repo.GetAsync(orderId, token)).ReturnsAsync((DbOrder)null!);
            var exceptionResult = await Assert.ThrowsAsync<NotFoundException>(async () => await _orderService.GetAsync(orderId, token));
            _orderRepository.Verify(repo => repo.GetAsync(orderId, token), Times.Once);

            Assert.NotNull(exceptionResult);
            Assert.IsType<NotFoundException>(exceptionResult);
            Assert.Equal($"Order# '{orderId}' not found.", exceptionResult.Message);
        }

        [Fact]
        public async void OrderService_GetAsync_ThrowUnexpectedException()
        {
            var orderId = _fixture.Create<Guid>();
            var token = _fixture.Create<CancellationToken>();
            var exception = _fixture.Create<Exception>();

            _orderRepository.Setup(repo => repo.GetAsync(orderId, token)).ThrowsAsync(exception);
            var exceptionResult = await Assert.ThrowsAsync<Exception>(async () => await _orderService.GetAsync(orderId, token));
            _orderRepository.Verify(repo => repo.GetAsync(orderId, token), Times.Once);

            Assert.NotNull(exceptionResult);
            Assert.IsType<Exception>(exceptionResult);
            Assert.Equal(exception, exceptionResult);
        }

        [Fact]
        public async void OrderService_GetAsync_ReturnValidResult()
        {
            var orderId = _fixture.Create<Guid>();
            var orderResult = _fixture.Create<DbOrder>();
            var token = _fixture.Create<CancellationToken>();

            _orderRepository.Setup(repo => repo.GetAsync(orderId, token)).ReturnsAsync(orderResult);
            var result = await _orderService.GetAsync(orderId, token);
            _orderRepository.Verify(repo => repo.GetAsync(orderId, token), Times.Once);

            Assert.NotNull(result);
            Assert.IsType<ContractOrder>(result);
            Assert.Equal(orderResult.Id, result.Id);
            Assert.Equal(orderResult.Details, result.Details);
        }

        [Fact]
        public async void OrderService_CreateAsync_ThrowArgumentNullException()
        {
            var order = (ContractOrder)null!;
            var token = _fixture.Create<CancellationToken>();

            var exceptionResult = await Assert.ThrowsAsync<ArgumentNullException>(async () => await _orderService.CreateAsync(order, token));
            _orderRepository.Verify(repo => repo.CreateAsync(It.IsAny<DbOrder>(), token), Times.Never);
            _rabbitTemplate.Verify(template => template.SendAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IMessage>(), token), Times.Never);

            Assert.NotNull(exceptionResult);
            Assert.IsType<ArgumentNullException>(exceptionResult);
            Assert.Equal($"Value cannot be null. (Parameter '{nameof(order)}')", exceptionResult.Message);
        }

        [Fact]
        public async void OrderService_CreateAsync_ThrowUnexpectedException()
        {
            var order = _fixture.Create<ContractOrder>();
            var token = _fixture.Create<CancellationToken>();
            var exception = _fixture.Create<Exception>();

            _orderRepository.Setup(repo => repo.CreateAsync(It.IsAny<DbOrder>(), token)).Throws(exception);
            var exceptionResult = await Assert.ThrowsAsync<Exception>(async () => await _orderService.CreateAsync(order, token));
            _orderRepository.Verify(repo => repo.CreateAsync(It.IsAny<DbOrder>(), token), Times.Once);
            _rabbitTemplate.Verify(template => template.SendAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IMessage>(), token), Times.Never);

            Assert.NotNull(exceptionResult);
            Assert.IsType<Exception>(exceptionResult);
            Assert.Equal(exception, exceptionResult);
        }

        [Fact]
        public async void OrderService_CreateAsync_ThrowRabbitUnexpectedException()
        {
            var order = _fixture.Create<ContractOrder>();
            var resultOrder = _fixture.Create<DbOrder>();
            var token = _fixture.Create<CancellationToken>();
            var exception = _fixture.Create<Exception>();

            _orderRepository.Setup(repo => repo.CreateAsync(It.IsAny<DbOrder>(), token)).ReturnsAsync(resultOrder);
            _rabbitTemplate.Setup(template => template.SendAsync(RabbitMqNameTemplates.NewOrder, "1.0", It.IsAny<IMessage>(), token)).Throws(exception);
            var exceptionResult = await Assert.ThrowsAsync<Exception>(async () => await _orderService.CreateAsync(order, token));
            _orderRepository.Verify(repo => repo.CreateAsync(It.IsAny<DbOrder>(), token), Times.Once);
            _rabbitTemplate.Verify(template => template.SendAsync(RabbitMqNameTemplates.NewOrder, "1.0", It.IsAny<IMessage>(), token), Times.Once);

            Assert.NotNull(exceptionResult);
            Assert.IsType<Exception>(exceptionResult);
            Assert.Equal(exception, exceptionResult);
        }

        [Fact]
        public async void OrderService_CreateAsync_ReturnValidResult()
        {
            var order = _fixture.Create<ContractOrder>();
            var resultOrder = _fixture.Create<DbOrder>();
            var token = _fixture.Create<CancellationToken>();

            _orderRepository.Setup(repo => repo.CreateAsync(It.IsAny<DbOrder>(), token)).ReturnsAsync(resultOrder);
            var result = await _orderService.CreateAsync(order, token);
            _orderRepository.Verify(repo => repo.CreateAsync(It.IsAny<DbOrder>(), token), Times.Once);
            _rabbitTemplate.Verify(template => template.SendAsync(RabbitMqNameTemplates.NewOrder, "1.0", It.IsAny<IMessage>(), token), Times.Once);

            Assert.NotNull(result);
            Assert.IsType<ContractOrder>(result);
            Assert.Equal(resultOrder.Id, result.Id);
            Assert.Equal(resultOrder.Details, result.Details);
        }
    }
}