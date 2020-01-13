using System;
using Core;
using Core.Enumerations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.Ledger
{
  [TestClass]
  public class WhenALedgerLineIsCreated
  {
    private readonly LedgerLine _myLedgerLine;

    public WhenALedgerLineIsCreated()
    {
      var mainCurrencyPair = new CurrencyPair(ValidCurrencies.LiteCoin, 20.0, ValidCurrencies.BitCoin, 1.00);
      _myLedgerLine = new LedgerLine("0001",TradeOrderType.Buy, 100.0, mainCurrencyPair, new DateTime(2014, 1, 1, 5, 30, 15));
    }

    [TestMethod]
    public void It_should_know_the_date_and_time_of_the_transaction()
    {
      var transactionDateTime = new DateTime(2014, 1, 1, 5, 30, 15);
      Assert.AreEqual(transactionDateTime.Date, _myLedgerLine.WhenTransaction.Date);
    }
  }
}