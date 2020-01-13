using Core;
using Core.Enumerations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ExchangeInteraction.OrderBookManagement
{
  [TestClass]
  public class WhenIGetOrderDataFromBtce :TestApiResponseBase
  {
    private OrderBook _myOrderBook;

    [TestMethod]
    public void It_can_give_me_a_sell_order_book()
    {
      _myOrderBook = OrderBookEngine.GetAnOrderBook(TradeOrderType.Sell, BtcSampleJson, Exchange.BtcE, ValidCurrencies.BitCoin, ValidCurrencies.UsDollars);
      Assert.AreEqual(_myOrderBook.TheTradeType, TradeOrderType.Sell);
      Assert.AreEqual(_myOrderBook.TheBook[0].ValueOutGoing(), 0.1157);
      Assert.AreEqual(_myOrderBook.TheBook[0].ValueInIncoming().ToString(), 67.6723515.ToString());
    }

    [TestMethod]
    public void It_can_give_me_a_buy_order_book()
    {
      _myOrderBook = OrderBookEngine.GetAnOrderBook(TradeOrderType.Buy, BtcSampleJson, Exchange.BtcE, ValidCurrencies.BitCoin, ValidCurrencies.UsDollars);
      Assert.AreEqual(_myOrderBook.TheTradeType, TradeOrderType.Buy);
      Assert.AreEqual(_myOrderBook.TheBook[0].ValueOutGoing(), 0.5673);
      Assert.AreEqual(_myOrderBook.TheBook[0].ValueInIncoming().ToString(), 330.7359.ToString());
    }
  }
}
