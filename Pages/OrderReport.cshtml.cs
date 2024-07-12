using bakery.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace bakery.Pages
{
    public class OrderReportModel : PageModel
    {

        public List<Order> Orders { get; set; }
        public void OnGet()
        {
        }
    }
}
