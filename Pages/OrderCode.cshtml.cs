using bakery.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace bakery.Pages
{
    // [ModelBinder]
    public class OrderCodeModel : PageModel
    {
        private BakeryContext context;
        private readonly IWebHostEnvironment hostingEnvironment;

        public OrderCodeModel(BakeryContext context,Microsoft.AspNetCore.Hosting.IWebHostEnvironment hostingEnvironment)
        {
            this.context = context;
            this.hostingEnvironment = hostingEnvironment;
        }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }
        //public Product Product { get; set; }
        [BindProperty, Range(1, 10, ErrorMessage = "You must order at least one item & max 10")]
        public int Quantity { get; set; } = 1;
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
        [BindProperty,Display(Name ="Select Week")]
        public string Weak { get; set; }
        //[BindProperty,Display(Name ="Subscribe news letter nullable")]
        //public bool? SubscribeNewsLetterNullable { get; set; }
        //[BindProperty,Display(Name ="Are checked")]
        //public List<int> AreChecked{ get; set; }
        [BindProperty,Display(Name ="Upload/Update new file")]
        public IFormFile UploadedFile { get; set; }
        public void OnGet()
        {
            
        }
        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                context.Orders.Add(new Models.Order()
                {
                    OrderEmail = OrderEmail,
                    Quantity = Quantity,
                    ShippingAddress = ShippingAddress,
                    SubscribeNewsLetter = SubscribeNewsLetter,
                    UnitPrice = UnitPrice
                });
                context.SaveChanges();
                var pathUploaded = Path.Combine(hostingEnvironment.ContentRootPath,"uploads",UploadedFile.FileName);
                using (var stream = new FileStream(pathUploaded, FileMode.Create))
                {
                    UploadedFile.CopyTo(stream);
                }
                return Redirect("OrderReport");
            }
            return Page();
        }
    }
}
