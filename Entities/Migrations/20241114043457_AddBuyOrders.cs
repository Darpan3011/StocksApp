using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    public partial class AddBuyOrders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string buyOrderProcedure = @"
                CREATE PROCEDURE [dbo].[addBuyOrders]
                    @BuyOrderID UNIQUEIDENTIFIER,
                    @StockSymbol NVARCHAR(50),
                    @StockName NVARCHAR(100),
                    @DateAndTimeOfOrder DATETIME,
                    @Quantity INT,
                    @Price FLOAT
                AS
                BEGIN
                    INSERT INTO BuyOrderTable (BuyOrderID, StockSymbol, StockName, DateAndTimeOfOrder, Quantity, Price)
                    VALUES (@BuyOrderID, @StockSymbol, @StockName, @DateAndTimeOfOrder, @Quantity, @Price)
                END
            ";

            migrationBuilder.Sql(buyOrderProcedure);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE [dbo].[addBuyOrders]");
        }
    }
}