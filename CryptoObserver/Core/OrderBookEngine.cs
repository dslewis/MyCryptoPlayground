using System;
using System.Linq;
using Core.ApiComponents;
using Core.Enumerations;
using Newtonsoft.Json;

namespace Core
{
  public class OrderBookEngine
  {
    public static OrderBook GetAnOrderBook(TradeOrderType tradeType, string serializedResponse, Exchange fromExchange,
      ValidCurrencies outgoing, ValidCurrencies incoming)
    {
      var myNewBook = new OrderBook(tradeType);
      switch (fromExchange)
      {
        case Exchange.BtcE:
          myNewBook = BtcEBookBuilder(tradeType, myNewBook, serializedResponse, outgoing, incoming);
          break;
      }
      return myNewBook;
    }

    private static OrderBook BtcEBookBuilder(TradeOrderType tradeType, OrderBook myNewBook, string data,
      ValidCurrencies outgoing, ValidCurrencies incoming)
    {
      var dataSet = JsonConvert.DeserializeObject<BtcEOrdersResult>(data, new JsonSerializerSettings { MissingMemberHandling = MissingMemberHandling.Error });
      if (tradeType == TradeOrderType.Sell)
      {
        foreach (var currentOrder in from sellOrder in dataSet.Asks
          let currentPairing = new CurrencyPair(outgoing, 1.00, incoming, Convert.ToDouble(sellOrder[0]))
          select new TradeOrder(TradeOrderType.Sell, Convert.ToDouble(sellOrder[1]), currentPairing))
        {
          myNewBook.AddOrder(currentOrder);
        }
      }
      else
      {
        foreach (var currentOrder in from buyOrder in dataSet.Bids
          let currentPairing = new CurrencyPair(outgoing, 1.00, incoming, Convert.ToDouble(buyOrder[0]))
          select new TradeOrder(TradeOrderType.Buy, Convert.ToDouble(buyOrder[1]), currentPairing))
        {
          myNewBook.AddOrder(currentOrder);
        }
      }
      return myNewBook;
    }
  }
}