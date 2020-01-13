using System.Collections.Generic;
using Core;
using Core.Enumerations;

namespace UnitTests.Framework
{
  public class FactoryMethods
  {
    public static List<CurrencyPair> CurrencyPairTestFactory(CurrencyPair startingPair, int numberofPairstoCreate,
      bool devalueright)
    {
      var factor = devalueright ? 0.9 : 1.1;
      var pairs = new List<CurrencyPair>
      {
        startingPair
      };
      --numberofPairstoCreate;
      var newValue = startingPair.RightCurrencyValue*factor;
      while (numberofPairstoCreate > 0)
      {
        pairs.Add(new CurrencyPair(startingPair.LeftCurrencySymbol, startingPair.LeftCurrencyValue,
          startingPair.RightCurrencySymbol, newValue));
        numberofPairstoCreate--;
        newValue = newValue*factor;
      }
      return pairs;
    }

    public static OrderBook OrderBookTestFiller(TradeOrder startingOrder, int transactionCount, double increment)
    {
      var book = new OrderBook(startingOrder.TradeType);
      var sampleOrders = TradeOrderTestFactory(startingOrder, transactionCount, increment);
      foreach (var sampleOrder in sampleOrders)
      {
        book.AddOrder(sampleOrder);
      }
      return book;
    }

    public static IEnumerable<TradeOrder> TradeOrderTestFactory(TradeOrder startingOrder, int numberOfOrdersToCreate,
      double increment)
    {
      var devalueRight = startingOrder.TradeType == TradeOrderType.Buy;
      var indexer = 1;
      var pairs = CurrencyPairTestFactory(startingOrder.TradeCurrencyPair, numberOfOrdersToCreate, devalueRight);
      var orders = new List<TradeOrder>
      {
        startingOrder
      };
      --numberOfOrdersToCreate;
      while (numberOfOrdersToCreate > 0)
      {
        orders.Add(new TradeOrder(startingOrder.TradeType, increment, pairs[indexer]));
        indexer++;
        numberOfOrdersToCreate--;
      }
      return orders;
    }
  }
}