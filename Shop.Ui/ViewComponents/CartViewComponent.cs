using Microsoft.AspNetCore.Mvc;
using Shop.Application.Cart;
using Shop.Database;

namespace Shop.Ui.ViewComponents
{
    public class CartViewComponent:ViewComponent
    {
        private GetCart getCart;

        public CartViewComponent(GetCart _getCart)
        {
            getCart= _getCart;
        }

        public IViewComponentResult Invoke(string view = "Default"
            )
        {
            if(view =="small")
            {
                var totalValue=getCart.Do().Sum(x=>x.RealValue*x.Qty);

                return View(view,$"Rs.{totalValue}");
            }
            return View(view,getCart.Do());
        }
    }
}
