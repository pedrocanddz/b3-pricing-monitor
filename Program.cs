using B3PricingMonitor;

//Recieving Args
var pricingToMonitor = args[0];
var lowPrice = decimal.Parse(args[1], System.Globalization.CultureInfo.InvariantCulture);
var highPrice = decimal.Parse(args[2], System.Globalization.CultureInfo.InvariantCulture);


//Creating HTTPClient and Observer structure
var netWork = new Network(Environment.GetEnvironmentVariable("API_KEY"));
var stockMonitor = new StockMonitor();
var stockAlertObserver = new StockAlertObserver(lowPrice, highPrice);
stockMonitor.Subscribe(stockAlertObserver);

while (true)
{
    //Fetching Stock and notifying Observers
    var stock = await netWork.GetStockAsync(pricingToMonitor, "1m", "1d");
    stockMonitor.Notify(stock);

    await Task.Delay(10000);
}


