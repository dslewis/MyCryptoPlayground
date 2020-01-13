using System;
using Core;
using Core.Enumerations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.Ledger
{
  [TestClass]
  public class WhenITryToUpsertATransactionWithADifferentTradeType : LedgerBase
  {
    public WhenITryToUpsertATransactionWithADifferentTradeType()
    {
      FodderLedgerLine = new LedgerLine("abc", TradeOrderType.Buy, 2.0,
      new CurrencyPair(ValidCurrencies.BitCoin, 1.0, ValidCurrencies.UsDollars, 500.0),
      new DateTime(2014, 4, 1, 9, 30, 00));
      FodderLedger.UpsertLine(FodderLedgerLine);

      FodderLedgerLine = new LedgerLine("abc", TradeOrderType.Sell, 1.0,
        new CurrencyPair(ValidCurrencies.BitCoin, 1.0, ValidCurrencies.UsDollars, 600.0),
        new DateTime(2014, 4, 2, 9, 30, 00));
    }

    [TestMethod]
    public void It_should_throw_an_invalid_operation_exception()
    {
      try
      {
        FodderLedger.UpsertLine(FodderLedgerLine);
        Assert.Fail();
      }
      catch (InvalidOperationException e)
      {
        Assert.AreEqual("You cannot change the trade order type of an existing transaction.", e.Message);
      }
    }
  }
}