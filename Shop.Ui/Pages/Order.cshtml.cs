using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shop.Application.Cart;
using Shop.Database;
using Shop.Application.Orders;

namespace Shop.Ui.Pages
{
    public class OrderModel : PageModel
    {
        private ApplicationDbContext _ctx;

        public OrderModel(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }
        public Shop.Application.Orders.GetOrder.Response Order { get; set; }
        public void OnGet(string reference)
        {
          Order=  new Shop.Application.Orders.GetOrder(_ctx).Do(reference);
        }

    }
}
