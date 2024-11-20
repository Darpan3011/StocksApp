using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace UnitTesting_UDEMY.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet("Error")]
        public IActionResult Error()
        {
            IExceptionHandlerPathFeature? iexpcetion = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            if(iexpcetion != null && iexpcetion.Error.Message != null)
            {
                ViewBag.ErrorMessage = iexpcetion.Error.Message;
            }
            return View();
        }
    }
}
