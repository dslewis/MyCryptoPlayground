using System;
using Core.Enumerations;
using Core.Exceptions;

namespace Core
{
  public class TradeOrder
  {
    private readonly double _valueInOutgoing;
    public CurrencyPair TradeCurrencyPair;
    public TradeOrderType TradeType;

    protected TradeOrder()
    {
    }

    public TradeOrder(TradeOrderType toTrade, double amountToTrade, CurrencyPair currencyPair)
    {
      if (amountToTrade <= 0.00)
      {
        var errorDescription = String.Format(
          "Trade Order cannot be created with amount {0}.  It less than or equal to zero.",
          amountToTrade);
        throw new MalformedTradeOrderException(errorDescription);
      }
      if (currencyPair == null)
      {
        throw new InvalidOperationException("Currency pair was null in TradeOrder construction");
      }
      _valueInOutgoing = amountToTrade;
      TradeCurrencyPair = currencyPair;
      TradeType = toTrade;
    }

    public double ValueInIncoming()
    {
      return TradeCurrencyPair.RightFromLeft(_valueInOutgoing);
    }

    public double ValueOutGoing()
    {
      return _valueInOutgoing;
    }
  }
}