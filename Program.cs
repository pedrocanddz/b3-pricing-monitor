using B3PricingMonitor;


var pricingToMonitor = args[0];
var lowPrice = args[1];
var highPrice = args[2];

var netWork = new Network(Environment.GetEnvironmentVariable("API_KEY"));
var stock = await netWork.GetStockAsync(pricingToMonitor, "1m", "1d");
Console.WriteLine(stock.RegularMarketPrice);    

