using bakery.CassandraMappings;
using bakery.Data;
using bakery.HttpClientTypes;
using bakery.Services;
using Cassandra;
using Cassandra.Mapping;
using Microsoft.AspNetCore.Hosting;
using System.Net.Http.Headers;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using bakery.Pages;
using Polly;
using Polly.Extensions.Http;
using Polly.Retry;
using System.Text;
using ClassLibrary1;

//var tuple = new Tuple<string, string>("fd", "fd");
//var tuple1 = new Tuple<string, string>("fd", "fd");

int[] arr = [1, 4, 45, 2, 10, 8,4,8];
int target = 16;
if(arr.Length < 2)
{

}
HashSet<int> list = new HashSet<int>(arr);
for (int i = 0; i < arr.Length - 1; i++)
{
    if(arr[i] < target)
    //if(Array.IndexOf(arr,target - arr[i],i + 1) > -1) { } 
    {
        var co = target - arr[i];
        if (list.Contains(co))
        {

        }
        if (list.Contains(co) && co != arr[i])
        {

        }
    }
    
}

Dictionary<string, int> dict1 = new Dictionary<string, int>
        {
            { "app", 1 },
            { "banana", 2 },
            { "cherry", 3 }
        };

Dictionary<string, int> dict2 = new Dictionary<string, int>
        {
            { "apple", 1 },
            { "banana", 2 },
            { "cherry", 3 }
        };

bool areEqual = dict1.OrderBy(kvp => kvp.Key).SequenceEqual(dict2.OrderBy(kvp => kvp.Key));



ClassH.Main();
var builder = WebApplication.CreateBuilder(args);
var inp = "       abc         skldcd        dkwc           ";
// 
var rev = inp.Split(' ',StringSplitOptions.RemoveEmptyEntries).ext();
//string.Join(" ", rev);
int position = 0;
bool inserted = true;
StringBuilder stringBuilder = new StringBuilder();
foreach (var kv in rev)
{

}
foreach (var line in inp)
{
    if (line == ' ')
    {
        stringBuilder.Append(line);
        inserted = true;
    }
    else if(inserted) { stringBuilder.Append(rev[position]); position++; inserted = false; }
}
// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<BakeryContext>();
builder.Services.AddTransient<CorrelationHandler>();
builder.Services.AddSingleton<LayoutService>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddMediatR(c => c.RegisterServicesFromAssemblies(typeof(Program).Assembly));


https://www.milanjovanovic.tech/blog/extending-httpclient-with-delegating-handlers-in-aspnetcore


builder.Services.AddTransient<AuthorizationHandler>();
builder.Services.AddHttpClient(nameof(PremierProductModelWrapper), ConfigureHttpClient());
builder.Services.AddHttpClient(nameof(OrderReportModel), ConfigHttpClient).AddHttpMessageHandler(()=> new CorrelationHandler())
    .AddHttpMessageHandler<CorrelationHandler>().AddPolicyHandler(GetRetryPolicy());
//builder.Services.AddHttpClient<GoogleClient>(client => client.BaseAddress = new Uri("https://google.com")).AddHttpMessageHandler<AuthorizationHandler>().AddStandardResilienceHandler();

MappingConfiguration.Global.Define<CassandraMapping>();
static void ConfigHttpClient(HttpClient httpClient)
{
    httpClient.Timeout = TimeSpan.FromSeconds(300);
    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
}

 static Action<HttpClient> ConfigureHttpClient()
{
    return option =>
    {
        option.Timeout = TimeSpan.FromSeconds(30);
        option.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
    };
}

static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
{
    return HttpPolicyExtensions.HandleTransientHttpError().OrResult(response => response.StatusCode == System.Net.HttpStatusCode.NotFound)
        .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
}
static AsyncRetryPolicy JitterExponentialBackoff()
{
    Random jitterer = new Random();
    return Policy.Handle<Exception>().WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))  // exponential back-off: 2, 4, 8 etc
                    + TimeSpan.FromMilliseconds(jitterer.Next(0, 10000)));// plus some jitter: up to 1 second)
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


public class CorrelationHandler : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.Headers.Add("trace_id", Guid.NewGuid().ToString());
        return await base.SendAsync(request, cancellationToken);
    }
}
public static class C
{
    public static string[] ext(this  string[] s)
    {
        var h = new string[s.Length];
        int j = 0;
        foreach (var i in s)
        {
          h[j] = new string(i.Reverse().ToArray());
            j++;
        }
        return h;
    }
}



