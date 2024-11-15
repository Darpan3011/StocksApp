using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    public partial class AddSellOrders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string sellOrderProcedure = @"
                CREATE PROCEDURE [dbo].[addSellOrders]
                    @SellOrderID UNIQUEIDENTIFIER,
                    @StockSymbol NVARCHAR(50),
                    @StockName NVARCHAR(100),
                    @DateAndTimeOfOrder DATETIME,
                    @Quantity INT,
                    @Price FLOAT
                AS
                BEGIN
                    INSERT INTO SellOrderTable (SellOrderID, StockSymbol, StockName, DateAndTimeOfOrder, Quantity, Price)
                    VALUES (@SellOrderID, @StockSymbol, @StockName, @DateAndTimeOfOrder, @Quantity, @Price)
                END
            ";

            migrationBuilder.Sql(sellOrderProcedure);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE [dbo].[addSellOrders]");
        }
    }
}