using System;
using System.Collections.Generic;
using System.Linq;
using Core.Enumerations;
using Core.Exceptions;

namespace Core
{
  public class OrderBook
  {
    public readonly List<TradeOrder> TheBook;
    public readonly TradeOrderType TheTradeType;

    public OrderBook(TradeOrderType tradeType)
    {
      TheBook = new List<TradeOrder>();
      TheTradeType = tradeType;
    }

    public void AddOrder(TradeOrder tradeOrder)
    {
      ValidateIncomingOrder(tradeOrder);
      TheBook.Add(tradeOrder);
    }

    public GainLossLedger BuildTransactions(double depth, GainLossLedger ledgerToAppend)
    {
      var toAppend = ledgerToAppend;
      var indexer = 0;
      var tradeType = TheBook[0].TradeType;
      while (depth > 0.0)
      {
        if (depth < TheBook[indexer].ValueOutGoing())
        {
          toAppend.Transactions.Add(new LedgerLine(Guid.NewGuid().ToString(), tradeType, depth,
            TheBook[indexer].TradeCurrencyPair, DateTime.Now));
          depth = 0.0;
        }
        else
        {
          toAppend.Transactions.Add(new LedgerLine(Guid.NewGuid().ToString(), tradeType,
            TheBook[indexer].ValueOutGoing(), TheBook[indexer].TradeCurrencyPair, DateTime.Now));
          depth = depth - TheBook[indexer].ValueOutGoing();
          indexer++;
        }
      }
      return toAppend;
    }

    public double GetIncomingPrice(double amountOfOutGoingCurrency)
    {
      if (amountOfOutGoingCurrency <= 0.00)
      {
        var errorDescription = String.Format("The amount you are trying to analyze {0} must be greater than zero.",
          amountOfOutGoingCurrency);
        throw new MalformedCalculationException(errorDescription);
      }

      var totalIncoming = 0.00;
      var coveredOutgoing = 0.00;
      var tradeOrderIndex = 0;
      while (tradeOrderIndex < TheBook.Count)
      {
        if (!(TheBook[tradeOrderIndex].ValueOutGoing() >= (amountOfOutGoingCurrency - coveredOutgoing)))
        {
          totalIncoming +=
            TheBook[tradeOrderIndex].ValueInIncoming();
          coveredOutgoing += TheBook[tradeOrderIndex].ValueOutGoing();
          tradeOrderIndex++;
        }
        else
        {
          totalIncoming +=
            TheBook[tradeOrderIndex].TradeCurrencyPair.RightFromLeft(amountOfOutGoingCurrency - coveredOutgoing);
          return totalIncoming;
        }
      }
      var overrunDescription = String.Format("Your request of {0} exceeds the order book's total volume of {1}",
        amountOfOutGoingCurrency, coveredOutgoing);
      throw new OrderBookOverrunException(overrunDescription);
    }

    public ValidCurrencies? IncomingCurrency()
    {
      if (TheBook[0] == null) return null;
      return TheBook[0].TradeCurrencyPair.LeftCurrencySymbol;
    }

    public ValidCurrencies? OutgoingCurrency()
    {
      if (TheBook[0] == null) return null;
      return TheBook[0].TradeCurrencyPair.RightCurrencySymbol;
    }

    public int TradeCount()
    {
      return TheBook.Count;
    }

    private void ValidateIncomingOrder(TradeOrder incoming)
    {
      if (incoming.TradeType != TheTradeType)
      {
        var errorDescription = String.Format(
          "This order book cannot take a {0} order.  It already has at least one {1} order.",
          incoming.TradeType,
          TheTradeType);
        throw new MalformedOrderBookException(errorDescription);
      }
      if (TheBook.Count <= 0) return;
      var firstOrDefault = TheBook.FirstOrDefault();
      if (firstOrDefault != null && ((firstOrDefault.TradeCurrencyPair.LeftCurrencySymbol ==
                                      incoming.TradeCurrencyPair.LeftCurrencySymbol) &&
                                     (firstOrDefault.TradeCurrencyPair.RightCurrencySymbol ==
                                      incoming.TradeCurrencyPair.RightCurrencySymbol))) return;
      var tradeOrder = TheBook.FirstOrDefault();
      if (tradeOrder == null) return;
      var errorDescription2 = String.Format(
        "CurrencyPair {0}, {1} cannot enter the order book.  It already has at least one pair of {2}, {3}",
        incoming.TradeCurrencyPair.LeftCurrencySymbol,
        incoming.TradeCurrencyPair.RightCurrencySymbol,
        tradeOrder.TradeCurrencyPair.LeftCurrencySymbol,
        tradeOrder.TradeCurrencyPair.RightCurrencySymbol);
      throw new MalformedOrderBookException(errorDescription2);
    }
  }
}