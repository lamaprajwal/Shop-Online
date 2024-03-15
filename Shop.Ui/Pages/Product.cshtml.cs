using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Shop.Application.Cart;
using Shop.Application.Products;
using Shop.Database;
 

namespace Shop.Ui.Pages
{
    public class ProductModel : PageModel
    {
        private ApplicationDbContext _ctx;
        public ProductModel(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }
        [BindProperty]
        public AddToCart.Request CartViewModel { get; set; }
        
        [BindProperty]
        public Shop.Domain.Models.Image img {  get; set; }

        
        public GetProduct.ProductViewModel Product { get; set; }
        public async Task<IActionResult> OnGet(string name)
        {
            Product =await new GetProduct(_ctx).Do(name);
             img= await _ctx.Images.FirstOrDefaultAsync(x => x.ProductId == Product.id);
            
            if(Product == null)
            {
                return RedirectToPage("Index");
            }
            else
            {
                return Page();
            }
        }
        public async Task<IActionResult> OnPost(
            [FromServices] AddToCart addToCart)
        {
          var result= await   addToCart.Do(CartViewModel);
            return RedirectToPage("Cart");
        }
    }
}
