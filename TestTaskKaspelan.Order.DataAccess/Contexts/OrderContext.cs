using Microsoft.EntityFrameworkCore;
using DbOrder = TestTaskKaspelan.Order.DataAccess.Entities.Order;

namespace TestTaskKaspelan.Order.DataAccess.Contexts
{
    /// <summary>
    /// Order database context.
    /// <seealso cref="Microsoft.EntityFrameworkCore.DbContext"/> successor.
    /// </summary>
    public class OrderContext : DbContext
    {
        /// <summary>
        /// Dbset for tree nodes.
        /// </summary>
        public DbSet<DbOrder> Orders { get; set; }

        /// <summary>
        /// Conscructor of <seealso cref="TestTaskKaspelan.Order.DataAccess.Contexts.OrderContext"/>.
        /// </summary>
        /// <param name="options">Configuration options.</param>
        public OrderContext(DbContextOptions<OrderContext> options)
            : base(options)
        {
        }
    }
}
