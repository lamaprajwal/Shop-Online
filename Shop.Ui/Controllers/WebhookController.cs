using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace Shop.Ui.Controllers
{
    public class WebhookController : Controller
    {
        [Route("webhook")]
        [ApiController]
        public class WebhookControll : Controller
        {

            // This is your Stripe CLI webhook secret for testing your endpoint locally.
            const string endpointSecret = "whsec_8139634aede9e461b8f6060a08ed86c064dd7629c2997f67499249f666aa9178";

            [HttpPost]
            public async Task<IActionResult> Index()
            {
                var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
                try
                {
                    var stripeEvent = EventUtility.ConstructEvent(json,
                        Request.Headers["Stripe-Signature"], endpointSecret);

                    // Handle the event
                    if (stripeEvent.Type == Events.PaymentIntentSucceeded)
                    {
                        return Ok(stripeEvent);
                    }
                    
                    else
                    {
                        Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
                    }

                    return Ok();
                }
                catch (StripeException e)
                {
                    return BadRequest();
                }
            }
        }
    }
}
