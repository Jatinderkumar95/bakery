using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace bakery.Pages.Shared.Components.Basket;

public class BasketViewComponent : ViewComponent
{
 public IViewComponentResult Invoke(){
    Models.Basket basket = new Models.Basket();

        if (Request.Cookies[nameof(Basket)] is not null)
        {
            basket = JsonSerializer.Deserialize<Models.Basket?>(Request.Cookies[nameof(Basket)]!);
        }
        else{
           basket = new Models.Basket(){
            Items = new List<string>(){"abc"},
            NumberOfItems = 5
           } ;
        }
        return View(basket);
 }
}
