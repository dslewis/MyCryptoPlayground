using System;
using Core;
using Core.Enumerations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.Ledger
{
  [TestClass]
  public class WhenICalculateCapitalGains : LedgerBase
  {
    public WhenICalculateCapitalGains()
    {
      FodderLedgerLine = new LedgerLine("0001",TradeOrderType.Buy, 1.0,
        new CurrencyPair(ValidCurrencies.BitCoin, 1.0, ValidCurrencies.UsDollars, 500.0),
        new DateTime(2014, 4, 1, 9, 30, 00));
      FodderLedger.UpsertLine(FodderLedgerLine);

      FodderLedgerLine = new LedgerLine("0002",TradeOrderType.Buy, 1.0,
        new CurrencyPair(ValidCurrencies.BitCoin, 1.0, ValidCurrencies.UsDollars, 600.0),
        new DateTime(2014, 4, 2, 9, 30, 00));
      FodderLedger.UpsertLine(FodderLedgerLine);

      FodderLedgerLine = new LedgerLine("0003",TradeOrderType.Buy, 10.0,
        new CurrencyPair(ValidCurrencies.LiteCoin, 1.0, ValidCurrencies.UsDollars, 30.0),
        new DateTime(2014, 4, 3, 9, 30, 00));
      FodderLedger.UpsertLine(FodderLedgerLine);

      FodderLedgerLine = new LedgerLine("0004",TradeOrderType.Buy, 10.0,
        new CurrencyPair(ValidCurrencies.LiteCoin, 1.0, ValidCurrencies.UsDollars, 25.0),
        new DateTime(2014, 4, 4, 9, 30, 00));
      FodderLedger.UpsertLine(FodderLedgerLine);

      FodderLedgerLine = new LedgerLine("0005",TradeOrderType.Sell, 15.0,
        new CurrencyPair(ValidCurrencies.LiteCoin, 1.0, ValidCurrencies.UsDollars, 20.0),
        new DateTime(2014, 4, 5, 9, 30, 00));
      FodderLedger.UpsertLine(FodderLedgerLine);

      FodderLedgerLine = new LedgerLine("0006",TradeOrderType.Sell, 1.50,
        new CurrencyPair(ValidCurrencies.BitCoin, 1.0, ValidCurrencies.UsDollars, 1100.0),
        new DateTime(2014, 4, 6, 9, 30, 00));
      FodderLedger.UpsertLine(FodderLedgerLine);
    }

    [TestMethod]
    public void It_Should_return_the_capital_gains_for_the_entire_ledger()
    {
      Assert.AreEqual(725.00, FodderLedger.CalculateGainOrLossForLedger(LongAgo, FarFromNow).ShortTermGain);
    }

    [TestMethod]
    public void It_should_return_the_capital_gains_for_a_currency()
    {
      Assert.AreEqual(850.00,
        FodderLedger.CalculateGainOrLoss(ValidCurrencies.BitCoin, LongAgo, FarFromNow).ShortTermGain);
    }
  }
}