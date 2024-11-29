using bakery.CassandraMappings;
using bakery.Data;
using bakery.HttpClientTypes;
using bakery.Services;
using Cassandra;
using Cassandra.Mapping;
using System.Net.Http.Headers;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<BakeryContext>();
builder.Services.AddSingleton<LayoutService>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();


https://www.milanjovanovic.tech/blog/extending-httpclient-with-delegating-handlers-in-aspnetcore


builder.Services.AddTransient<AuthorizationHandler>();
builder.Services.AddHttpClient(nameof(PremierProductModelWrapper), ConfigureHttpClient());
//builder.Services.AddHttpClient<GoogleClient>(client => client.BaseAddress = new Uri("https://google.com")).AddHttpMessageHandler<AuthorizationHandler>().AddStandardResilienceHandler();

MappingConfiguration.Global.Define<CassandraMapping>();
 static Action<HttpClient> ConfigureHttpClient()
{
    return option =>
    {
        option.Timeout = TimeSpan.FromSeconds(30);
        option.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
    };
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();



