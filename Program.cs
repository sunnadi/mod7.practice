using System;
using System.Collections.Generic;

public interface IObserver
{
    void Update(string stockName, decimal newPrice);  
}

public interface ISubject
{
    void Subscribe(IObserver obs, string stock);   
    void Unsubscribe(IObserver obs, string stock); 
    void Notify(string stock);                     
}

public class StockExchange : ISubject
{
    private Dictionary<string, decimal> prices = new Dictionary<string, decimal>();
    private Dictionary<string, List<IObserver>> subscribers = new Dictionary<string, List<IObserver>>();

    public void Subscribe(IObserver obs, string stock)
    {
        if (!subscribers.ContainsKey(stock))
        {
            subscribers[stock] = new List<IObserver>();  
        }
        subscribers[stock].Add(obs);  
    }

    public void Unsubscribe(IObserver obs, string stock)
    {
        if (subscribers.ContainsKey(stock))
        {
            subscribers[stock].Remove(obs);  
        }
    }

    public void Notify(string stock)
    {
        if (subscribers.ContainsKey(stock))
        {
            foreach (var obs in subscribers[stock])
            {
                obs.Update(stock, prices[stock]);  
            }
        }
    }

    public void UpdatePrice(string stock, decimal newPrice)
    {
        prices[stock] = newPrice;  
        Notify(stock);             
    }
}

public class Trader : IObserver
{
    private string name;  

    public Trader(string traderName)
    {
        name = traderName;
    }

    public void Update(string stockName, decimal newPrice)
    {
        Console.WriteLine($"Трейдер {name} получил уведомление: Акция {stockName} теперь стоит {newPrice}.");
    }
}

public class RobotTrader : IObserver
{
    public void Update(string stockName, decimal newPrice)
    {
        if (newPrice > 100)
        {
            Console.WriteLine($"Робот покупает акцию {stockName}, так как цена выше 100.");
        }
        else
        {
            Console.WriteLine($"Робот продает акцию {stockName}, так как цена ниже или равна 100.");
        }
    }
}


class Program
{
    static void Main(string[] args)
    {
        StockExchange exchange = new StockExchange();

        Trader trader1 = new Trader("Lidiya");
        Trader trader2 = new Trader("Stylez");
        RobotTrader robot = new RobotTrader();

        exchange.Subscribe(trader1, "AAPL");
        exchange.Subscribe(trader2, "AAPL");
        exchange.Subscribe(robot, "AAPL");

        exchange.UpdatePrice("AAPL", 150);
        exchange.UpdatePrice("AAPL", 90);

        exchange.Unsubscribe(trader2, "AAPL");

        exchange.UpdatePrice("AAPL", 120);
    }
}
