using AutoFixture;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using TestTaskKaspelan.Order.DataAccess.Contexts;
using TestTaskKaspelan.Order.DataAccess.Repositories;
using TestTaskKaspelan.Order.DataAccess.Interfaces;
using DbOrder = TestTaskKaspelan.Order.DataAccess.Entities.Order;

namespace TestTaskKaspelan.Order.DataAccess.UnitTests.Repositories
{
    public class OrderRepositoryUnitTests
    {
        Fixture _fixture;
        Mock<ILogger<OrderRepository>> _logger;
        IOrderRepository _orderRepository;
        OrderContext _orderContext;
        ServiceProvider _serviceProvider;

        public OrderRepositoryUnitTests()
        {
            _fixture = new Fixture();
            var configuration = new Mock<IConfiguration>();
            var configurationSection = new Mock<IConfigurationSection>();
            configurationSection.Setup(section => section[It.IsAny<string>()]).Returns("");
            configuration.Setup(config => config.GetSection(It.IsAny<string>())).Returns(configurationSection.Object);
            var services = new ServiceCollection();
            services.AddScoped<IConfiguration>((provider) => configuration.Object);
            services.AddDbContext<OrderContext>(options =>
                options.UseInMemoryDatabase("TestDb"));
            _serviceProvider = services.BuildServiceProvider();
            _orderContext = _serviceProvider.GetRequiredService<OrderContext>();
            _logger = new Mock<ILogger<OrderRepository>>();
            _orderRepository = new OrderRepository(_orderContext, _logger.Object);
        }

        [Fact]
        public async void OrderRepository_CreateAsync_ThrowArgumentNullException()
        {
            var order = (DbOrder)null!;
            var token = _fixture.Create<CancellationToken>();

            var exceptionResult = await Assert.ThrowsAsync<ArgumentNullException>(async () => await _orderRepository.CreateAsync(order, token));

            Assert.NotNull(exceptionResult);
            Assert.IsType<ArgumentNullException>(exceptionResult);
            Assert.Equal($"Value cannot be null. (Parameter '{nameof(order)}')", exceptionResult.Message);
        }

        [Fact]
        public async void OrderRepository_CreateAsync_ReturnValidResult()
        {
            var order = _fixture.Create<DbOrder>();
            using var tokenSource = new CancellationTokenSource();

            var result = await _orderRepository.CreateAsync(order, tokenSource.Token);

            Assert.NotNull(result);
            Assert.IsType<DbOrder>(result);
            Assert.Equal(order.Id, result.Id);
            Assert.Equal(order.Details, result.Details);
            Assert.Equal(order, _orderContext.Orders.Find([order.Id]));
        }

        [Fact]
        public async void OrderRepository_GetAsync_ArgumentException()
        {
            var orderId = Guid.Empty;
            var token = _fixture.Create<CancellationToken>();

            var exceptionResult = await Assert.ThrowsAsync<ArgumentException>(async () => await _orderRepository.GetAsync(orderId, token));

            Assert.NotNull(exceptionResult);
            Assert.IsType<ArgumentException>(exceptionResult);
            Assert.Equal(nameof(orderId), exceptionResult.Message);
        }

        [Fact]
        public async void OrderRepository_GetAsync_ReturnValidResult()
        {
            var order = _fixture.Create<DbOrder>();
            using var tokenSource = new CancellationTokenSource();

            await _orderRepository.CreateAsync(order, tokenSource.Token);
            var foundResult = await _orderRepository.GetAsync(order.Id, tokenSource.Token);

            Assert.NotNull(foundResult);
            Assert.IsType<DbOrder>(foundResult);
            Assert.Equal(order.Id, foundResult.Id);
            Assert.Equal(order.Details, foundResult.Details);
            Assert.Equal(order, _orderContext.Orders.Find([foundResult.Id]));
        }

        [Fact]
        public async void OrderRepository_GetAsync_ReturnNull()
        {
            var orderId = Guid.NewGuid();
            using var tokenSource = new CancellationTokenSource();

            var foundResult = await _orderRepository.GetAsync(orderId, tokenSource.Token);

            Assert.Null(foundResult);
        }

        ~OrderRepositoryUnitTests()
        {
            if(_orderContext != null)
            {
                _orderContext.Dispose();
            }
        }
    }
}