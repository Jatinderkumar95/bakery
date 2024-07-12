using System.ComponentModel.DataAnnotations;
using bakery.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Primitives;

namespace bakery.Pages
{
    // [ModelBinder]
    public class OrderCodeModel : PageModel
    {
        private BakeryContext context;
        public OrderCodeModel(BakeryContext context) => this.context = context;
        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }
        //public Product Product { get; set; }
        [BindProperty, Range(1, int.MaxValue, ErrorMessage = "You must order at least one item")]
        public int Quantity { get; set; }
        //EmailAddress - It only validates that the submitted string includes just one "@" character within it, and it is neither the first nor the last character.
        [BindProperty, Required, EmailAddress, Display(Name = "Your Email Address")]
        public string OrderEmail { get; set; }
        [BindProperty, Required, Display(Name = "Shipping Address")]
        public string ShippingAddress { get; set; }
        [BindProperty]
        public decimal UnitPrice { get; set; }
        [TempData]
        public string Confirmation { get; set; }
        //Most browsers will understand any value as true, so even the following will result in the checkbox being checked:
        //<input type="checkbox" checked="false">
        [BindProperty,Display(Name ="Subscribe news letter")]
        public bool SubscribeNewsLetter { get; set; }
        //[BindProperty,Display(Name ="Subscribe news letter nullable")]
        //public bool? SubscribeNewsLetterNullable { get; set; }
        //[BindProperty,Display(Name ="Are checked")]
        //public List<int> AreChecked{ get; set; }

        public void OnGet()
        {
            
        }
        public void OnPost()
        {
            if (!ModelState.IsValid)
            {
                return;
            }
        }
    }
}
