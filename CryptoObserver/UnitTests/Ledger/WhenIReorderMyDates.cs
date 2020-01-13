using System;
using Core;
using Core.Enumerations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.Ledger
{
  [TestClass]
  public class WhenIReorderMyDates : LedgerBase
  {
    private readonly DateTime _oldestDate;
    private readonly DateTime _newestDate;
    private readonly double _oldestValue;
    private readonly double _middleValue;


    public WhenIReorderMyDates()
    {
      _oldestDate = new DateTime(2011, 4, 1, 9, 30, 00);
      _newestDate = new DateTime(2014, 4, 1, 9, 30, 00);
      _oldestValue = 500.0;
      _middleValue = 600.0;

      FodderLedgerLine = new LedgerLine("0001",TradeOrderType.Buy, 1.0,
        new CurrencyPair(ValidCurrencies.BitCoin, 1.0, ValidCurrencies.UsDollars, _middleValue),
        new DateTime(2011, 5, 1, 9, 30, 00));
      FodderLedger.UpsertLine(FodderLedgerLine);

      FodderLedgerLine = new LedgerLine("0002",TradeOrderType.Buy, 1.0,
        new CurrencyPair(ValidCurrencies.BitCoin, 1.0, ValidCurrencies.UsDollars, 700.0),
        _newestDate);
      FodderLedger.UpsertLine(FodderLedgerLine);

      FodderLedgerLine = new LedgerLine("0003",TradeOrderType.Buy, 1.0,
        new CurrencyPair(ValidCurrencies.BitCoin, 1.0, ValidCurrencies.UsDollars, _oldestValue),
        _oldestDate);
      FodderLedger.UpsertLine(FodderLedgerLine);

      FodderLedger.OrderByDate();
    }

    [TestMethod]
    public void It_should_put_my_entry_dates_in_order()
    {
      Assert.AreEqual(FodderLedger.Transactions[0].WhenTransaction, _oldestDate);
      Assert.AreEqual(FodderLedger.Transactions[2].WhenTransaction, _newestDate);
    }

    [TestMethod]
    public void It_should_reflect_the_currency_amounts_in_the_right_order()
    {
      Assert.AreEqual(FodderLedger.Transactions[0].TradeCurrencyPair.RightCurrencyValue, _oldestValue);
      Assert.AreEqual(FodderLedger.Transactions[1].TradeCurrencyPair.RightCurrencyValue, _middleValue);
    }
  }
}