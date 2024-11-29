namespace bakery.Services
{
    public class PremierProductModelWrapper
    {
        public PremierProductModelWrapper(IHttpClientFactory httpClientFactory)
        {
            HttpClientFactory = httpClientFactory;
        }

        public IHttpClientFactory HttpClientFactory { get; }

        public async Task Testabc()
        {
            try
            {
                var httpClient = HttpClientFactory.CreateClient(nameof(PremierProductModelWrapper));
                httpClient.DefaultRequestHeaders.Add("Cookie", string.Empty);
                await httpClient.GetAsync("https://google.com", default(CancellationToken));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
            
        }
    }
}
