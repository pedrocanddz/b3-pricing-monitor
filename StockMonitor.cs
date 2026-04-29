namespace B3PricingMonitor;

public sealed class StockMonitor : IObservable<Stock>
{
    private readonly List<IObserver<Stock>> _observers = new();

    public IDisposable Subscribe(IObserver<Stock> observer)
    {
        if(!_observers.Contains(observer))
            _observers.Add(observer);

        return new Unsubscriber<Stock>(_observers, observer);
    }
}

internal class Unsubscriber<T> : IDisposable
    {
        private readonly List<IObserver<T>> _observers;
        private readonly IObserver<T> _observer;

        public Unsubscriber(List<IObserver<T>> observers, IObserver<T> observer)
        {
            _observers = observers;
            _observer = observer;
        }

        public void Dispose()
        {
            if (_observers.Contains(_observer))
                _observers.Remove(_observer);
        }
    }