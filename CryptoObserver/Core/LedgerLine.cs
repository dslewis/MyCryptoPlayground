using System;
using Core.Enumerations;

namespace Core
{
  public class LedgerLine : TradeOrder
  {
    public LedgerLine()
    {
    }

    public LedgerLine(string id, TradeOrderType tradeOrderType, double amount, CurrencyPair mainCurrencyPair,
      DateTime whenTransaction) : base(tradeOrderType, amount, mainCurrencyPair)
    {
      TradeType = tradeOrderType;
      Amount = amount;
      WhenTransaction = whenTransaction;
      Id = id;
    }

    public DateTime WhenTransaction { get; set; }

    public double Amount { get; set; }

    public string Id { get; set; }
  }
}