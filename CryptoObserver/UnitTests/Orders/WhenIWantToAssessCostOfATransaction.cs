using Core;
using Core.Enumerations;
using Core.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.Orders
{
  [TestClass]
  public class WhenIWantToAssessCostOfATransaction
  {
    private readonly OrderBook _fodderOrderBook;


    public WhenIWantToAssessCostOfATransaction()
    {
      var fodderCurrencyPair1 = new CurrencyPair(ValidCurrencies.BitCoin, 1.00, ValidCurrencies.UsDollars, 500.00);
      var fodderCurrencyPair2 = new CurrencyPair(ValidCurrencies.BitCoin, 1.00, ValidCurrencies.UsDollars, 510.00);
      var fodderCurrencyPair3 = new CurrencyPair(ValidCurrencies.BitCoin, 1.00, ValidCurrencies.UsDollars, 520.00);
      var fodderTradeOrder1 = new TradeOrder(TradeOrderType.Sell, 2.00, fodderCurrencyPair1);
      var fodderTradeOrder2 = new TradeOrder(TradeOrderType.Sell, 1.00, fodderCurrencyPair2);
      var fodderTradeOrder3 = new TradeOrder(TradeOrderType.Sell, 1.00, fodderCurrencyPair3);
      _fodderOrderBook = new OrderBook(TradeOrderType.Sell);
      _fodderOrderBook.AddOrder(fodderTradeOrder1);
      _fodderOrderBook.AddOrder(fodderTradeOrder2);
      _fodderOrderBook.AddOrder(fodderTradeOrder3);
    }

    [TestMethod]
    public void It_can_give_me_a_price_in_the_incoming_currency()
    {
      Assert.AreEqual(1000.00, _fodderOrderBook.GetIncomingPrice(2.00));
    }

    [TestMethod]
    public void It_can_slip_through_multiple_orders_to_build_a_price()
    {
      Assert.AreEqual(1770.00, _fodderOrderBook.GetIncomingPrice(3.50));
    }

    [TestMethod]
    public void It_should_not_allow_a_zero_or_negative_outgoing_amount()
    {
      try
      {
        _fodderOrderBook.GetIncomingPrice(0.00);
        Assert.Fail();
      }
      catch (MalformedCalculationException e)
      {
        Assert.AreEqual("The amount you are trying to analyze 0 must be greater than zero.", e.Message);
      }
      try
      {
        _fodderOrderBook.GetIncomingPrice(-4.00);
        Assert.Fail();
      }
      catch (MalformedCalculationException e)
      {
        Assert.AreEqual("The amount you are trying to analyze -4 must be greater than zero.", e.Message);
      }
    }

    [TestMethod]
    public void It_should_not_calculate_if_the_ammount_is_larger_than_the_order_book()
    {
      try
      {
        _fodderOrderBook.GetIncomingPrice(100.00);
        Assert.Fail();
      }
      catch (OrderBookOverrunException e)
      {
        Assert.AreEqual("Your request of 100 exceeds the order book's total volume of 4", e.Message);
      }
    }
  }
}