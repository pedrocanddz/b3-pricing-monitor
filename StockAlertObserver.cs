namespace B3PricingMonitor;
public class StockAlertObserver : IObserver<Stock>
{
    public decimal LowPrice {get;set;}
    public decimal HighPrice {get; set;}
    public List<String> Emails{get; set;} = new();

    public StockAlertObserver(decimal lowPrice, decimal highPrice, List<String> emails)
    {
        LowPrice = lowPrice;
        HighPrice = highPrice;
        Emails.AddRange(emails);
    }

    public void OnCompleted()
    {
        Console.WriteLine("Fluxo completo");
    }

    public void OnError(Exception error)
    {
        Console.WriteLine("Erro em Observer: " + error.Message);
    }

    public void OnNext(Stock value)
    {
        if(value.RegularMarketPrice > this.HighPrice)
            Console.WriteLine("Valor passou do Limite superior. Disparar email");
            
        if(value.RegularMarketPrice < this.LowPrice)
            Console.WriteLine("Valor passou do Limite inferior. Disparar email");
    }
}