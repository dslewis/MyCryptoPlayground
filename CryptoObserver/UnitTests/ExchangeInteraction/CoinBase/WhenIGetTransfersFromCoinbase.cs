using Core;
using Core.Enumerations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTests.ExchangeInteraction.OrderBookManagement;

namespace UnitTests.ExchangeInteraction.CoinBase
{
  [TestClass]
  public class WhenIGetTransfersFromCoinbase : TestApiResponseBase
  {
    private readonly GainLossLedger _ledger;

    public WhenIGetTransfersFromCoinbase()
    {
      _ledger = TransferEngine.CoinbaseLedgerUpdater(CoinBaseTransferSampleJson, new GainLossLedger());
      _ledger.OrderByDate();
    }

    [TestMethod]
    public void It_should_account_for_fees_when_valuing_a_buy()
    {
      Assert.AreEqual(_ledger.Transactions[0].Amount, 1.0);
      Assert.AreEqual(_ledger.Transactions[0].TradeCurrencyPair.RightCurrencyValue, 13.84);
    }

    [TestMethod]
    public void It_should_account_for_fees_when_valuing_a_sell()
    {
      Assert.AreEqual(_ledger.Transactions[1].TradeCurrencyPair.RightCurrencyValue, 13.26);
    }

    [TestMethod]
    public void It_should_have_the_correct_Id()
    {
      Assert.AreEqual("5011f33df8182b142400000e", _ledger.Transactions[0].Id);
    }

    [TestMethod]
    public void It_should_have_the_correct_date_stamp()
    {
      Assert.AreEqual("2/28/2013 2:28:18 AM", _ledger.Transactions[0].WhenTransaction.ToString());
    }

    [TestMethod]
    public void It_should_have_the_correct_trade_order_type()
    {
      Assert.AreEqual(TradeOrderType.Buy, _ledger.Transactions[0].TradeType);
    }
  }
}