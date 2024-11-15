using Entities.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Entities
{
    public class OrdersDbContext : DbContext
    {
        public OrdersDbContext(DbContextOptions options) : base(options) { }
        public DbSet<BuyOrder> buyOrders { get; set; }
        public DbSet<SellOrder> sellOrders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BuyOrder>().ToTable("BuyOrderTable");
            modelBuilder.Entity<SellOrder>().ToTable("SellOrderTable");
        }

        public int sp_AddBuyOrder(BuyOrder buyOrder)
        {
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@BuyOrderID", buyOrder.BuyOrderID),
                new SqlParameter("@StockSymbol", buyOrder.StockSymbol),
                new SqlParameter("@StockName", buyOrder.StockName),
                new SqlParameter("@DateAndTimeOfOrder", buyOrder.DateAndTimeOfOrder),
                new SqlParameter("@Quantity", buyOrder.Quantity),
                new SqlParameter("@Price", buyOrder.Price),
            };

            return Database.ExecuteSqlRaw(
                "EXECUTE [dbo].[addBuyOrders] @BuyOrderID, @StockSymbol, @StockName, @DateAndTimeOfOrder, @Quantity, @Price", sp);
        }

        public int sp_AddSellOrder(SellOrder sellOrder)
        {
            // Define the SQL parameters to pass to the stored procedure
            SqlParameter[] sp = new SqlParameter[]
            {
                new SqlParameter("@SellOrderID", sellOrder.SellOrderID),
                new SqlParameter("@StockSymbol", sellOrder.StockSymbol),
                new SqlParameter("@StockName", sellOrder.StockName),
                new SqlParameter("@DateAndTimeOfOrder", sellOrder.DateAndTimeOfOrder),
                new SqlParameter("@Quantity", sellOrder.Quantity),
                new SqlParameter("@Price", sellOrder.Price)
            };

            // Execute the stored procedure and return the result (number of rows affected)
            return Database.ExecuteSqlRaw("EXEC [dbo].[addSellOrders] @SellOrderID, @StockSymbol, @StockName, @DateAndTimeOfOrder, @Quantity, @Price", sp);
        }

    }

}