using Microsoft.AspNetCore.Mvc;
using Shop.Application.Cart;
using Shop.Database;

namespace Shop.Ui.Controllers
{

    [Route("[controller]/[action]")]
    public class CartController : Controller
    {
       

        [HttpPost("{stockId}")]
        public async Task <IActionResult>AddOne(int stockId,
            [FromServices]AddToCart addToCart)
        {
            var request=new AddToCart.Request
            { StockId=stockId,
               Qty=1};
           
            var success = await addToCart.Do(request);
            if(success)
            {
            return Ok("Removed from cart");

            }
            return BadRequest("Failed to remove from  cart");
        }
        [HttpPost("{stockId}")]
        public async Task<IActionResult> RemoveOne(int stockId,
            [FromServices] RemoveFromCart removeFromCart)
        {
            var request = new RemoveFromCart.Request
            {
                StockId = stockId,
                Qty = 1
            };
           
            var success = await removeFromCart.Do(request);
            if (success)
            {
                return Ok("Item Added to cart");

            }
            return BadRequest("Failed to add to cart");
        }

        [HttpPost("{stockId}")]
        public async Task<IActionResult> RemoveAll(int stockId,
            [FromServices] RemoveFromCart removeFromCart)
        {
            var request = new RemoveFromCart.Request
            {
                StockId = stockId,
                All=true
            };
           
            var success = await removeFromCart.Do(request);
            if (success)
            {
                return Ok("Item removed all from cart");

            }
            return BadRequest("Failed to remove all items from cart");
        }
    }
}
