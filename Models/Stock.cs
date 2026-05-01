using System.Text.Json.Serialization;
public class Stock
{
    public required string Symbol { get; set; }
    public required string LongName { get; set; }
    public required string Currency { get; set; }
    public decimal RegularMarketPrice { get; set; }
    public decimal RegularMarketChangePercent { get; set; }
    public DateTime RegularMarketTime { get; set; }
    public decimal RegularMarketPreviousClose { get; set; }
    public required string LogoUrl { get; set; }
}   

public class APIResponse
{
    [JsonPropertyName("results")]
    public required List<Stock> Results {get; set;}
} 