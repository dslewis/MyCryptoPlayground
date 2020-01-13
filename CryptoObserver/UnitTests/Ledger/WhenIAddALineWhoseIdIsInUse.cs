
using System;
using Core;
using Core.Enumerations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.Ledger
{
  [TestClass]
  public class WhenIAddALineWhoseIdIsInUse : LedgerBase
  {
    public WhenIAddALineWhoseIdIsInUse()
    {
      FodderLedgerLine = new LedgerLine("abc", TradeOrderType.Buy, 2.0,
        new CurrencyPair(ValidCurrencies.BitCoin, 1.0, ValidCurrencies.UsDollars, 500.0),
        new DateTime(2014, 4, 1, 9, 30, 00));
      FodderLedger.UpsertLine(FodderLedgerLine);

      FodderLedgerLine = new LedgerLine("abc", TradeOrderType.Buy, 1.0,
        new CurrencyPair(ValidCurrencies.BitCoin, 1.0, ValidCurrencies.UsDollars, 600.0),
        new DateTime(2014, 4, 2, 9, 30, 00));
      FodderLedger.UpsertLine(FodderLedgerLine);
    }

    [TestMethod]
    public void It_should_update_the_amount_to_reflect_the_new_value()
    {
      Assert.AreEqual(1.0, FodderLedger.Transactions[0].Amount);
    }

    [TestMethod]
    public void It_should_update_the_currency_pair_to_reflect_the_new_value()
    {
      Assert.AreEqual(FodderLedgerLine.TradeCurrencyPair, FodderLedger.Transactions[0].TradeCurrencyPair);
    }

    [TestMethod]
    public void It_should_update_the_datetime_to_reflect_the_new_value()
    {
      Assert.AreEqual(FodderLedgerLine.WhenTransaction, FodderLedger.Transactions[0].WhenTransaction);
    }

    [TestMethod]
    public void It_should_not_create_a_new_transaction()
    {
      Assert.AreEqual(1,FodderLedger.Transactions.Count);
    }
  }
}