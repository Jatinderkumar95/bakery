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

var builder = WebApplication.CreateBuilder(args);

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


