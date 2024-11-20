using Entities;
using Microsoft.AspNetCore.Mvc.Filters;
using UnitTesting_UDEMY.Controllers;

namespace UnitTesting_UDEMY.Filters
{
    public class ActionFilterForModelValidation : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            if (context.Controller is TradeController tradeController)
            {

                var order = context.ActionArguments["stockTrade"];

                if (!tradeController.ModelState.IsValid)
                {
                    foreach (var error in tradeController.ModelState.Values.SelectMany(v => v.Errors))
                    {
                        Console.WriteLine(error.ErrorMessage);
                    }

                    tradeController.ViewBag.ErrorMessage = "There was an error with your input. Please correct it and try again.";
                    context.Result = tradeController.View("Index", order);
                }
                else
                {
                    await next();
                }
            }
            else
            {
                await next();
            }
        }
    }
}