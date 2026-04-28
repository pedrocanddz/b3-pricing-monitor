using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.VisualBasic;

public class Stock
{
    public required string Symbol { get; set; }
    public required string ShortName { get; set; }
    public required string LongName { get; set; }
    public required string Currency { get; set; }
    public decimal RegularMarketPrice { get; set; }
    public decimal RegularMarketDayHigh { get; set; }
    public decimal RegularMarketDayLow { get; set; }
    public required string RegularMarketDayRange { get; set; }
    public decimal RegularMarketChange { get; set; }
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