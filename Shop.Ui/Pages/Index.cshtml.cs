using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shop.Application;
using Shop.Application.Products;

using Shop.Database;

namespace Shop.Ui.Pages

{
    public class IndexModel:PageModel
    {
        private ApplicationDbContext _ctx;
        public IndexModel(ApplicationDbContext ctx)
        {
            _ctx = ctx; 
            
        }
        public IEnumerable<GetProducts.ProductViewModel> Products { get; set; }
        [BindProperty]
        public List<Shop.Domain.Models.Image> img { get; set; }
        public void OnGet()
        {
            Products = new GetProducts(_ctx).Do();
            img = new List<Shop.Domain.Models.Image>();

            foreach (var product in Products)
            {
                var productImage = _ctx.Images.FirstOrDefault(x => x.ProductId == product.Id);
                if (productImage != null)
                {
                    img.Add(productImage); // Add the image to the list
                }
            }
           
        }
    }
}