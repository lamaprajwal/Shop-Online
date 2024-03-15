using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shop.Database;
using Shop.Domain.Models;
using Shop.Ui.FileManager;

namespace Shop.Ui.Pages.Admin
{
    public class AddImageModel : PageModel
    {
        private readonly ApplicationDbContext _ctx;
        private readonly IFileManager _fileManager;

        public AddImageModel(ApplicationDbContext ctx,IFileManager filemanager)
        {
            _ctx=ctx;
            _fileManager=filemanager;
        }
        public Image up_img { get; set; }
        [BindProperty]
       
        public ImageViewModel img { get; set; }

        public Shop.Domain.Models.Stripe _stripe { get; set; }

        [BindProperty]

        public StripeViewModel st { get; set; }
        
        public void OnGet()
        {

        }
        public async  Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                // Handle validation errors
                return Page();
            }
            var _simg = new Image
            {
                ImageId=img.ImageId,

                ProductId=img.ProductId,

                FileName=await _fileManager.SaveImage(img.FileName)
            };
            var _stripeS = new Shop.Domain.Models.Stripe
            {
                ProductId=img.ProductId,
                TestId=st.TestId

            };
            await _ctx.Stripes.AddAsync(_stripeS);
            await _ctx.SaveChangesAsync();
           await _ctx.Images.AddAsync(_simg);
            await _ctx.SaveChangesAsync();
            return RedirectToPage("/Admin/Index");

        }

        public class ImageViewModel
        {
            public int ImageId { get; set; }
            public IFormFile FileName { get; set; }
            // Other image properties

            public int ProductId { get; set; }
        }

        public class StripeViewModel
        {
            public string TestId { get; set; } // Stripe test ID property
                                               // Other properties...

            public int ProductId { get; set; }
        }
    }
}
