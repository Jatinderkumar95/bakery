using bakery.Data;
using bakery.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using static bakery.Pages.GetSystemTechSpecContentResponse;
using static bakery.Pages.OrderReportModel;

namespace bakery.Pages
{
    public class OrderReportModel : PageModel
    {
        readonly IHttpClientFactory _httpClientFactory;
        public OrderReportModel(IHttpClientFactory httpClientFactory, BakeryContext context)
        {
            _httpClientFactory = httpClientFactory;
            this.context = context;
        }
        private BakeryContext context;
        public List<Order> Orders { get; set; }
        public Lwp lwp { get; set; } = new Lwp()
        {
            Country = "fr",
            Language = "fr",
            Region = "emea",
            Segment = "bsd"
        };
        public async Task OnGet()
        {
            var request = new GetSystemTechSpecContentRequest()
            {
                Segment = lwp.Segment,
                ProductCodes = new List<string>() { "laptop" },
                SkipCache = true,
                Country = lwp.Country,
                ProductType = "Product",
                Language = lwp.Language,
                Region = lwp.Region
            };
            var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var httpCLient = _httpClientFactory.CreateClient(nameof(OrderReportModel));
            var response = await httpCLient.PostAsync("contentapi", default);
            var result = await response.Content?.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                SysProdTechSpec regulatory = new SysProdTechSpec();
                var contentStudioResponse = JsonSerializer.Deserialize<List<TechSpecContent>>(result, SetJsonSerializerOptions());
                if (contentStudioResponse != null && contentStudioResponse.Count != 0)
                    regulatory = GetParentSysProdTechSpecByTemplateNameId(contentStudioResponse, "reg");
            }
            Orders = context.Orders.ToList();
        }

        public SysProdTechSpec GetParentSysProdTechSpecByTemplateNameId(List<TechSpecContent> response, string templateNameId)
        {
            foreach (var techSpecContent in response)
            {
                foreach (var item in techSpecContent.Items)
                {
                    foreach (var sysProdTechSpec in item.Content.SysProdTechSpecs)
                    {
                        foreach (var templateName in sysProdTechSpec.TemplateName)
                        {
                            if (templateName.Id == templateNameId)
                            {
                                return sysProdTechSpec;
                            }
                        }
                    }
                }
            }
            return null; // Return null if no matching TemplateName is found
        }
        public class GetSystemTechSpecContentRequest

        {
            public string Country { get; set; }
            public string Language { get; set; }
            public string Segment { get; set; }
            public string Region { get; set; }
            public string ProductType { get; set; }
            public bool SkipCache { get; set; }
            public List<string> ProductCodes { get; set; }
        }
        public class Lwp
        {
            public string Country { get; set; }
            public string Language { get; set; }
            public string Segment { get; set; }
            public string Region { get; set; }
        }
        private static JsonSerializerOptions SetJsonSerializerOptions()
        {
            return new JsonSerializerOptions { PropertyNameCaseInsensitive = true, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };
        }
    }
    public class GetSystemTechSpecContentResponse
    {
        public List<TechSpecContent> TechSpecContents { get; set; }

        public class TechSpecContent
        {
            public List<Item> Items { get; set; }
        }

        public class Item
        {
            public Content Content { get; set; }
        }

        public class Content
        {
            public List<SysProdTechSpec> SysProdTechSpecs { get; set; }
        }

        public class SysProdTechSpec
        {
            public string SysTechSpecItemValue { get; set; }
            public List<Templatename> TemplateName { get; set; }
        }

        public class Templatename
        {
            public string CollectionId { get; set; }
            public string ElasticPath { get; set; }
            public string Id { get; set; }
            public string Name { get; set; }
        }
    }
}
