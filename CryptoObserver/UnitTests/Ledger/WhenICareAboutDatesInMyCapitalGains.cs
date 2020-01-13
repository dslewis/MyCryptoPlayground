using System;
using Core;
using Core.Enumerations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.Ledger
{
  [TestClass]
  public class WhenICareAboutDatesInMyCapitalGains : LedgerBase
  {
    public WhenICareAboutDatesInMyCapitalGains() : base()
    {
      FodderLedgerLine = new LedgerLine("0001",TradeOrderType.Buy, 1.0,
        new CurrencyPair(ValidCurrencies.BitCoin, 1.0, ValidCurrencies.UsDollars, 500.0),
        new DateTime(2011, 4, 1, 9, 30, 00));
      FodderLedger.UpsertLine(FodderLedgerLine);

      FodderLedgerLine = new LedgerLine("0002",TradeOrderType.Buy, 1.0,
        new CurrencyPair(ValidCurrencies.BitCoin, 1.0, ValidCurrencies.UsDollars, 600.0),
        new DateTime(2011, 5, 1, 9, 30, 00));
      FodderLedger.UpsertLine(FodderLedgerLine);

      FodderLedgerLine = new LedgerLine("0003",TradeOrderType.Buy, 1.0,
        new CurrencyPair(ValidCurrencies.BitCoin, 1.0, ValidCurrencies.UsDollars, 700.0),
        new DateTime(2014, 4, 1, 9, 30, 00));
      FodderLedger.UpsertLine(FodderLedgerLine);

      FodderLedgerLine = new LedgerLine("0004",TradeOrderType.Sell, 1.5,
        new CurrencyPair(ValidCurrencies.BitCoin, 1.0, ValidCurrencies.UsDollars, 800.0),
        new DateTime(2014, 5, 1, 9, 30, 00));
      FodderLedger.UpsertLine(FodderLedgerLine);

      FodderLedgerLine = new LedgerLine("0005",TradeOrderType.Sell, 1.5,
        new CurrencyPair(ValidCurrencies.BitCoin, 1.0, ValidCurrencies.UsDollars, 900.0),
        new DateTime(2014, 6, 1, 9, 30, 00));
      FodderLedger.UpsertLine(FodderLedgerLine);
    }

    [TestMethod]
    public void It_should_account_for_Long_and_Short_Term_Gains()
    {
      Assert.AreEqual(200.00, FodderLedger.CalculateGainOrLossForLedger(LongAgo, FarFromNow).ShortTermGain);
      Assert.AreEqual(550.00, FodderLedger.CalculateGainOrLossForLedger(LongAgo, FarFromNow).LongTermGain);
    }

    [TestMethod]
    public void It_should_only_calculate_sells_in_the_date_range()
    {
      Assert.AreEqual(0.00,
        FodderLedger.CalculateGainOrLossForLedger(new DateTime(2014, 4, 30), new DateTime(2014, 5, 10)).ShortTermGain);
      Assert.AreEqual(400.00,
        FodderLedger.CalculateGainOrLossForLedger(new DateTime(2014, 4, 30), new DateTime(2014, 5, 10)).LongTermGain);
    }
  }
}