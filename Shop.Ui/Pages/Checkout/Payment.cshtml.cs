using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using Shop.Application.Cart;
using Shop.Application.Infrastructure;
using Shop.Application.Orders;
using Shop.Database;
using Stripe;
using Stripe.Checkout;
using System.Security.Cryptography.X509Certificates;

namespace Shop.Ui.Pages.Checkout
{
    public class PaymentModel : PageModel
    {
        private ApplicationDbContext _ctx;

        public string PublicKey { get; }
        public string SecKey { get; }

        public PaymentModel(ApplicationDbContext ctx, IConfiguration config)
        {
            _ctx = ctx;
            PublicKey = config["Stripe:PublicKey"].ToString();
            SecKey = config["Stripe:SecretKey"].ToString();

        }
        public IActionResult OnGet([FromServices] GetCustomerInformation getcustomeri)

        {
            var information = getcustomeri.Do();
            if (information == null)
            {
                return RedirectToPage("/Checkout/CustomerInformation");
            }

            return Page();
        }  

            
               
               






            public async Task<IActionResult> OnPost(
                [FromServices] Shop.Application.Cart.GetOrder getOrder
                , [FromServices] ISessionManager sessionManager)
            {

                var CartOrder = getOrder.Do();


                var sessionId = HttpContext.Session.Id;
                await new CreateOrder(_ctx).Do(new CreateOrder.Request
                {
                    SessionId = sessionId,
                    FirstName = CartOrder.CustomerInformation.FirstName,
                    LastName = CartOrder.CustomerInformation.LastName,
                    Email = CartOrder.CustomerInformation.Email,
                    PhoneNumber = CartOrder.CustomerInformation.PhoneNumber,
                    Address1 = CartOrder.CustomerInformation.Address1,
                    Address2 = CartOrder.CustomerInformation.Address2,
                    City = CartOrder.CustomerInformation.City,
                    PostCode = CartOrder.CustomerInformation.PostCode,
                    Stocks = CartOrder.Products.Select(x => new CreateOrder.Stock
                    {
                        StockId = x.StockId,
                        Qty = x.Qty,

                    }).ToList()

                });

            var domain = "http://localhost:7148";
            var lineItems = new List<SessionLineItemOptions>();

            // Assuming CartOrder.GetItems() returns a list of items in the cart
            foreach (var item in CartOrder.Products)
            {
                var id = _ctx.Stripes.FirstOrDefault(x => x.ProductId == item.ProductId).TestId;
                var lineItem = new SessionLineItemOptions
                {
                    Price = id, // Use the price ID associated with the item
                    Quantity = item.Qty,
                };
                lineItems.Add(lineItem);
            }
            sessionManager.ClearCart();
            var options = new SessionCreateOptions
            {
                LineItems = lineItems,
                Mode = "payment",
                SuccessUrl = domain,
                CancelUrl = domain + "/cancel.html",
            };





            var service = new SessionService();
            Session session = service.Create(options);

            Response.Headers.Add("Location", session.Url);
            
            return new StatusCodeResult(303);




            return RedirectToPage("/Index");


            }

        }

    }





    