using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Shop.Application.Cart;
using Shop.Database;
using Shop.Domain.Models;

namespace Shop.Ui.Pages
{
    public class CartModel : PageModel
    {
        private ApplicationDbContext _ctx;

        public CartModel(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }
        public IEnumerable<GetCart.Response> Cart {  get; set; }
        [BindProperty]

        public List<Image> img { get; set; }
        public async Task<IActionResult> OnGet(
            [FromServices] GetCart getCart)
        {
            Cart=getCart.Do();
            img = new List<Shop.Domain.Models.Image>();

            foreach (var product in Cart)
            {
                var productImage = _ctx.Images.FirstOrDefault(x => x.ProductId == product.pid);
                if (productImage != null)
                {
                    img.Add(productImage); // Add the image to the list
                }
            }

            return Page();
        }
    }
}
