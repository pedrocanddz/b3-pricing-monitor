using B3PricingMonitor;
using Microsoft.Extensions.Configuration;

if (args.Length != 3)
{
    Console.Error.WriteLine("Para funcionamento correto é necessario passar tres argumentos <ATIVO> <PRECO_VENDA> <PRECO_COMPRA>");
    return;
}

var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

//Recieving and Parsing Args
var pricingToMonitor = args[0];
var lowPrice = decimal.Parse(args[2], System.Globalization.CultureInfo.InvariantCulture);
var highPrice = decimal.Parse(args[1], System.Globalization.CultureInfo.InvariantCulture);
var emailsToNotify = config.GetSection("Emails").Get<List<string>>();
var smtpConfig = config.GetSection("SmtpConfig").Get<SmtpConfig>();


//Creating HTTPClient and Observer structure
var netWork = new NetworkService(config["ApiKey"]);
var stockMonitor = new StockMonitor();
var stockAlertObserver = new StockAlertObserver(lowPrice, highPrice, emailsToNotify, smtpConfig);
stockMonitor.Subscribe(stockAlertObserver);

//Loop for monitoring
while (true)
{
    //Fetching Stock and notifying Observers
    var stock = await netWork.GetStockAsync(pricingToMonitor, "1m", "1d");
    stockMonitor.Notify(stock);

    await Task.Delay(100000);
}


