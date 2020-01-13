using System;
using Core;
using Core.Enumerations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.Forecasting
{
  [TestClass]
  public class WhenISimulateWithdrawingFromAnOrderBook
  {
    private GainLossLedger LedgerToDate;
    private readonly GainLossLedger SimulationLedger;
    private readonly CurrencyPair Pair1;
    private readonly CurrencyPair Pair2;
    private readonly CurrencyPair Pair3;

    public WhenISimulateWithdrawingFromAnOrderBook()
    {
      Pair1 = new CurrencyPair(ValidCurrencies.BitCoin, 1.0, ValidCurrencies.UsDollars, 500.0);
      Pair2 = new CurrencyPair(ValidCurrencies.BitCoin, 1.0, ValidCurrencies.UsDollars, 600.0);
      Pair3 = new CurrencyPair(ValidCurrencies.BitCoin, 1.0, ValidCurrencies.UsDollars, 700.0);

      LedgerToDate = new GainLossLedger();
      var startingTransaction = new LedgerLine(Guid.NewGuid().ToString(), TradeOrderType.Buy, 1.0,Pair1, DateTime.Now);
      LedgerToDate.Transactions.Add(startingTransaction);
      SimulationLedger = FodderBuyBook().BuildTransactions(2.0, LedgerToDate);
    }

    [TestMethod]
    public void It_should_build_the_transactions_needed_reach_the_depth()
    {
      Assert.AreEqual(0.5, SimulationLedger.Transactions[1].Amount);
      Assert.AreEqual(1.0, SimulationLedger.Transactions[2].Amount);
      Assert.AreEqual(0.5, SimulationLedger.Transactions[3].Amount);
      Assert.AreEqual(4, SimulationLedger.Transactions.Count);
    }

    private OrderBook FodderBuyBook()
    {
      var book = new OrderBook(TradeOrderType.Buy);
      book.AddOrder(new TradeOrder(TradeOrderType.Buy, 0.5, Pair1));
      book.AddOrder(new TradeOrder(TradeOrderType.Buy, 1.0, Pair2));
      book.AddOrder(new TradeOrder(TradeOrderType.Buy, 1.0, Pair3));
      return book;
    }
  }
}