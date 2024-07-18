using bakery.Data;
using bakery.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace bakery.Pages
{
    public class OrderReportModel : PageModel
    {
        private BakeryContext context;
        public OrderReportModel(BakeryContext context) => this.context = context;
        public List<Order> Orders { get; set; }
        public void OnGet()
        {
            Orders = context.Orders.ToList();
        }
    }
}
