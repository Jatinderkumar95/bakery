using bakery.Data;
using bakery.HttpClientTypes;
using bakery.Models;
using bakery.Services;
using Cassandra;
using Cassandra.Mapping;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net.Http;

namespace bakery.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private BakeryContext _bakeryContext;
    public List<Product> Products { get; set; }
    public IHttpClientFactory GoogleClient { get; set; }
    public IndexModel(ILogger<IndexModel> logger, BakeryContext bakeryContext, IHttpClientFactory googleClient)
    {
        _logger = logger;
        _bakeryContext = bakeryContext;
        GoogleClient = googleClient;
    }
    //https://juliocasal.com/blog/ASP.NET-Core-HttpClient-Tutorial
    //https://positiwise.com/blog/how-to-use-cookies-in-asp-net-core#:~:text=To%20do%20so%2C%20implement%20the,method%20with%20its%20overloaded%20version.
    public async void OnGet()
    {
        try
        {
            var googleCLient = GoogleClient.CreateClient(nameof(PremierProductModelWrapper));
            googleCLient.DefaultRequestHeaders.Add("test", string.Empty);
            var httpResponse = await googleCLient.GetAsync("https://google.com");
            Products = new List<Product>()
           {
               new Product(){ Id = 1, Description = "Bangel Recipe", ImageName = "bangel.jfif",Name = "Bangel",Price = 12.99m}
           };
                //_bakeryContext.Products.ToList();


            var cluster = Cluster.Builder()
                    .WithDefaultKeyspace("xx")
            //Each node stores a part of the data, and together, they form a cluster.
                    .AddContactPoint("xx")
                    .WithCredentials("xx", "xx")
                    .Build();
            //A cluster object maintains a permanent connection to one of the cluster node that it uses solely to maintain information on the state and current topology of the cluster. Using the connection, the driver will discover all the nodes composing the cluster as well as new nodes joining the cluster.
            var session = cluster.Connect("xx");

            // UDT
            await session.UserDefinedTypes.DefineAsync(
                UdtMap.For<CatalogKeybase>(udtName: "catalogkeybase",keyspace: "xx")
                .Automap()
            );

            // Mapper
            // IMapper mapper = new Mapper(session);
            // IEnumerable<ColorSwatch> colorSwatches = mapper.Fetch<ColorSwatch>("Select * from ColorSwatch where PageKey = ?","test");
            
            // IEnumerable<ColorSwatch> colorSwatches = mapper.Fetch<ColorSwatch>("Select * from ColorSwatch");
            // foreach (var row in colorSwatches)
            // {
            //     Console.WriteLine(row.ImageUrl);
            // }

            // For Querying
            // var statement = session.Prepare("Select * from ColorSwatch");
            // foreach(var row in session.Execute(statement.Bind())){
            //     Console.WriteLine(row.GetValue<string>("ImageUrl"));
            // }

            // For insert
            //session.Prepare("Insert into ")
        }
        catch (Exception ex)
        {
            Console.WriteLine(JsonConvert.SerializeObject(ex.Message));
        }
    }
}
