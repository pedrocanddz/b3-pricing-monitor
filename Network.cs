namespace B3PricingMonitor;

using System.Net.Http.Headers;
using System.Net.Http.Json;
public class Network
{
    public HttpClient Client;
    public Network(string apikey)
    {
        this.Client = new HttpClient();
        this.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apikey);
    }

    public async Task<Stock> GetStockAsync(string name, string interval, string range)
    {
        var response = await Client.GetAsync($"https://brapi.dev/api/quote/{name}?modules=summaryProfile&interval={interval}&range={range}");
        if (response.IsSuccessStatusCode)
        {
            try
            {
                var json = await response.Content.ReadFromJsonAsync<APIResponse>();
                if(json == null) throw new NullReferenceException("Resposta não lida corretamente, tentar novamente");
                return json.Results[0];
                
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        throw new Exception("Resposta diferente de 200 para API Brapi.");
        
    }
}