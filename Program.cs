using B3PricingMonitor;
using Microsoft.Extensions.Configuration;

var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

//Recieving Args
var pricingToMonitor = args[0];
var lowPrice = decimal.Parse(args[1], System.Globalization.CultureInfo.InvariantCulture);
var highPrice = decimal.Parse(args[2], System.Globalization.CultureInfo.InvariantCulture);
var emailsToNotify = config.GetSection("Emails").Get<List<string>>();


//Creating HTTPClient and Observer structure
var netWork = new Network(config["ApiKey"]);
var stockMonitor = new StockMonitor();
var stockAlertObserver = new StockAlertObserver(lowPrice, highPrice, emailsToNotify);
stockMonitor.Subscribe(stockAlertObserver);

while (true)
{
    //Fetching Stock and notifying Observers
    var stock = await netWork.GetStockAsync(pricingToMonitor, "1m", "1d");
    stockMonitor.Notify(stock);

    await Task.Delay(10000);
}


