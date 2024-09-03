using bakery.Data;
using bakery.Models;
using Cassandra;
using Cassandra.Mapping;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace bakery.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private BakeryContext _bakeryContext;
    public List<Product> Products { get; set; }
    public IndexModel(ILogger<IndexModel> logger, BakeryContext bakeryContext)
    {
        _logger = logger;
        _bakeryContext = bakeryContext;
    }

    public async void OnGet()
    {
        try
        {
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
