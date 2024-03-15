using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shop.Application.Cart;
using Shop.Database;

namespace Shop.Ui.Pages.Checkout
{
    public class CustomerInformationModel : PageModel
    {
        [BindProperty]
        public AddCustomerInformation.Request CustomerInformation {  get; set; }    
        public IActionResult OnGet([FromServices] GetCustomerInformation getCinfo)
        {
            //Get Customer Information
            var information = getCinfo.Do();
            if (information == null )
            {
                return Page();
            }
            else
            {
                return RedirectToPage("/Checkout/Payment");
            }
        }
        public IActionResult OnPost(
            [FromServices] AddCustomerInformation addCustomeri)
        {
            //Post Customer Information

            if(!ModelState.IsValid)
            {
                return Page();
            }
            addCustomeri.Do(CustomerInformation);
            return RedirectToPage("/Checkout/Payment");
        }
    }
}
