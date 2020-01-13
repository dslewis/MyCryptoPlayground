using Core;
using Core.Enumerations;
using Core.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.Orders
{
  [TestClass]
  public class WhenATradeOrderIsCreated
  {
    private CurrencyPair _myCurrencyPair;
    private TradeOrder _myTradeOrder;

    [TestMethod]
    public void It_should_know_the_amount_to_trade_in_the_incoming_currency()
    {
      _myCurrencyPair = new CurrencyPair(ValidCurrencies.LiteCoin, 40.00, ValidCurrencies.BitCoin, 1.00);
      _myTradeOrder = new TradeOrder(TradeOrderType.Sell, 100.00, _myCurrencyPair);
      Assert.AreEqual(_myTradeOrder.ValueInIncoming(), 2.5);
    }

    [TestMethod]
    public void It_should_know_the_amount_to_trade_in_the_outgoing_currency()
    {
      _myCurrencyPair = new CurrencyPair(ValidCurrencies.LiteCoin, 40.00, ValidCurrencies.BitCoin, 1.00);
      _myTradeOrder = new TradeOrder(TradeOrderType.Buy, 100.00, _myCurrencyPair);
      Assert.AreEqual(_myTradeOrder.ValueOutGoing(), 100.00);
    }

    [TestMethod]
    public void It_should_know_what_type_of_trade_it_is()
    {
      _myCurrencyPair = new CurrencyPair(ValidCurrencies.LiteCoin, 40.00, ValidCurrencies.BitCoin, 1.00);
      _myTradeOrder = new TradeOrder(TradeOrderType.Buy, 100.00, _myCurrencyPair);
      Assert.AreEqual(_myTradeOrder.TradeType, TradeOrderType.Buy);
    }

    [TestMethod]
    public void It_should_not_allow_the_trade_amount_to_be_zero_or_negative()
    {
      _myCurrencyPair = new CurrencyPair(ValidCurrencies.LiteCoin, 40.00, ValidCurrencies.BitCoin, 1.00);
      try
      {
        _myTradeOrder = new TradeOrder(TradeOrderType.Buy, 0.00, _myCurrencyPair);
        Assert.Fail();
      }
      catch (MalformedTradeOrderException e)
      {
        Assert.AreEqual("Trade Order cannot be created with amount 0.  It less than or equal to zero.", e.Message);
      }
      try
      {
        _myTradeOrder = new TradeOrder(TradeOrderType.Buy, -50.00, _myCurrencyPair);
        Assert.Fail();
      }
      catch (MalformedTradeOrderException e)
      {
        Assert.AreEqual("Trade Order cannot be created with amount -50.  It less than or equal to zero.", e.Message);
      }
    }
  }
}