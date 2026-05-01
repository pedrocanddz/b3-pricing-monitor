namespace B3PricingMonitor;
public class StockAlertObserver : IObserver<Stock>
{
    public decimal LowPrice { get; set; }
    public decimal HighPrice { get; set; }
    public List<string> Emails { get; set; } = [];
    public EmailService EmailService { get; set; }

    public StockAlertObserver(decimal lowPrice, decimal highPrice, List<string> emails, SmtpConfig smtpConfig)
    {
        LowPrice = lowPrice;
        HighPrice = highPrice;
        Emails.AddRange(emails);
        EmailService = new EmailService(smtpConfig);
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
        var marketTime = TimeZoneInfo.ConvertTimeFromUtc(
            DateTime.SpecifyKind(value.RegularMarketTime, DateTimeKind.Utc),
            TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time"));

        //Subject and Body messages mounted with AI
        if (value.RegularMarketPrice > HighPrice)
        {
            var subject = $"[{value.Symbol}] considere Vender (R$ {value.RegularMarketPrice:F2})";
            var body = BuildHtmlBody(value, marketTime, "acima do limite superior", "Limite superior", HighPrice);
            EmailService.SendAsync(Emails, subject, body).Wait();
        }

        if (value.RegularMarketPrice < LowPrice)
        {
            var subject = $"[{value.Symbol}] considere Comprar (R$ {value.RegularMarketPrice:F2})";
            var body = BuildHtmlBody(value, marketTime, "abaixo do limite inferior", "Limite inferior", LowPrice);
            EmailService.SendAsync(Emails, subject, body).Wait();
        }
    }

    private static string BuildHtmlBody(Stock s, DateTime marketTime, string alertLabel, string limitLabel, decimal limitValue) => $"""
        <!DOCTYPE html>
        <html lang="pt-BR">
        <body style="font-family:Arial,sans-serif;color:#222;max-width:520px;margin:auto">
          <div style="text-align:center;padding:16px 0">
            <img src="{s.LogoUrl}" alt="{s.Symbol}" style="height:56px;object-fit:contain" />
            <h2 style="margin:8px 0">{s.LongName} ({s.Symbol})</h2>
          </div>
          <hr/>
          <p>O preco atual de <strong>{s.LongName}</strong> esta <strong>{alertLabel}</strong>.</p>
          <table style="width:100%;border-collapse:collapse">
            <tr><td style="padding:6px 0;color:#555">Preco atual</td>
                <td style="padding:6px 0;font-weight:bold">R$ {s.RegularMarketPrice:F2}</td></tr>
            <tr><td style="padding:6px 0;color:#555">Fechamento anterior</td>
                <td style="padding:6px 0">R$ {s.RegularMarketPreviousClose:F2}</td></tr>
            <tr><td style="padding:6px 0;color:#555">Variacao do dia</td>
                <td style="padding:6px 0">{s.RegularMarketChangePercent:F2}%</td></tr>
            <tr><td style="padding:6px 0;color:#555">{limitLabel}</td>
                <td style="padding:6px 0">R$ {limitValue:F2}</td></tr>
            <tr><td style="padding:6px 0;color:#555">Horario</td>
                <td style="padding:6px 0">{marketTime:dd/MM/yyyy HH:mm:ss} (Horario de Brasilia)</td></tr>
          </table>
          <hr/>
          <p style="font-size:11px;color:#aaa;text-align:center">B3 Pricing Monitor</p>
        </body>
        </html>
        """;
}
