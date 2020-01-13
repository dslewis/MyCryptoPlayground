using System;
using Core;
using Core.Enumerations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.Ledger
{
  [TestClass]
  public class WhenIAddLedgerLines : LedgerBase
  {
    public WhenIAddLedgerLines()
    {
      FodderLedgerLine = new LedgerLine("0001", TradeOrderType.Buy, 2.0,
        new CurrencyPair(ValidCurrencies.BitCoin, 1.0, ValidCurrencies.UsDollars, 500.0),
        new DateTime(2014, 4, 1, 9, 30, 00));
      FodderLedger.UpsertLine(FodderLedgerLine);

      FodderLedgerLine = new LedgerLine("0002", TradeOrderType.Buy, 1.0,
        new CurrencyPair(ValidCurrencies.BitCoin, 1.0, ValidCurrencies.UsDollars, 600.0),
        new DateTime(2014, 4, 2, 9, 30, 00));
      FodderLedger.UpsertLine(FodderLedgerLine);

      FodderLedgerLine = new LedgerLine("0003", TradeOrderType.Buy, 100.0,
        new CurrencyPair(ValidCurrencies.LiteCoin, 1.0, ValidCurrencies.UsDollars, 600.0),
        new DateTime(2014, 4, 3, 9, 30, 00));
      FodderLedger.UpsertLine(FodderLedgerLine);

      FodderLedgerLine = new LedgerLine("0004", TradeOrderType.Sell, 2.00,
        new CurrencyPair(ValidCurrencies.BitCoin, 1.0, ValidCurrencies.UsDollars, 700.0),
        new DateTime(2014, 4, 4, 9, 30, 00));
      FodderLedger.UpsertLine(FodderLedgerLine);
    }

    [TestMethod]
    public void It_should_return_the_accumulated_value_of_a_valid_currency()
    {
      Assert.AreEqual(FodderLedger.CurrencyBalance(ValidCurrencies.BitCoin), 1.0);
    }
  }
}