using Core;
using Core.Enumerations;
using Core.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.Orders
{
  [TestClass]
  public class WhenIManageAnOrderBook
  {
    private OrderBook _myOrderBook;

    [TestMethod]
    public void I_should_be_able_to_add_trade_orders_to_it_after_creating_it()
    {
      _myOrderBook = new OrderBook(TradeOrderType.Buy);
      var currencyPair = new CurrencyPair(ValidCurrencies.BitCoin, 1.00, ValidCurrencies.LiteCoin, 40.00);
      var tradeOrder1 = new TradeOrder(TradeOrderType.Buy, 0.5, currencyPair);
      var tradeOrder2 = new TradeOrder(TradeOrderType.Buy, 1.0, currencyPair);
      _myOrderBook.AddOrder(tradeOrder1);
      _myOrderBook.AddOrder(tradeOrder2);
      Assert.AreEqual(_myOrderBook.TradeCount(), 2);
    }

    [TestMethod]
    public void I_should_not_able_able_to_add_trade_orders_with_different_currencies_in_their_pairs()
    {
      _myOrderBook = new OrderBook(TradeOrderType.Buy);
      var currencyPair1 = new CurrencyPair(ValidCurrencies.BitCoin, 1.00, ValidCurrencies.UsDollars, 500.00);
      var currencyPair2 = new CurrencyPair(ValidCurrencies.LiteCoin, 1.00, ValidCurrencies.UsDollars, 17.00);
      var tradeOrder1 = new TradeOrder(TradeOrderType.Buy, 0.5, currencyPair1);
      var tradeOrder2 = new TradeOrder(TradeOrderType.Buy, 1.0, currencyPair2);
      try
      {
        _myOrderBook.AddOrder(tradeOrder1);
        _myOrderBook.AddOrder(tradeOrder2);
        Assert.Fail();
      }
      catch (MalformedOrderBookException e)
      {
        Assert.AreEqual("CurrencyPair LiteCoin, UsDollars cannot enter the order book.  It already has at least one pair of BitCoin, UsDollars", e.Message);
      }
    }

    [TestMethod]
    public void I_should_not_be_able_to_put_more_than_one_type_of_trade_order_in_a_book()
    {
      _myOrderBook = new OrderBook(TradeOrderType.Buy);
      var currencyPair = new CurrencyPair(ValidCurrencies.BitCoin, 1.00, ValidCurrencies.LiteCoin, 40.00);
      var tradeOrder1 = new TradeOrder(TradeOrderType.Buy, 0.5, currencyPair);
      var tradeOrder2 = new TradeOrder(TradeOrderType.Sell, 1.0, currencyPair);
      try
      {
        _myOrderBook.AddOrder(tradeOrder1);
        _myOrderBook.AddOrder(tradeOrder2);
        Assert.Fail();
      }
      catch (MalformedOrderBookException e)
      {
        Assert.AreEqual("This order book cannot take a Sell order.  It already has at least one Buy order.", e.Message);
      }
    }

    [TestMethod]
    public void It_should_know_the_incoming_currency_of_the_book()
    {
      _myOrderBook = new OrderBook(TradeOrderType.Buy);
      var currencyPair = new CurrencyPair(ValidCurrencies.BitCoin, 1.00, ValidCurrencies.LiteCoin, 40.00);
      var tradeOrder1 = new TradeOrder(TradeOrderType.Buy, 0.5, currencyPair);
      _myOrderBook.AddOrder(tradeOrder1);
      Assert.AreEqual(ValidCurrencies.BitCoin, _myOrderBook.IncomingCurrency());
    }

    [TestMethod]
    public void It_should_know_the_outgoing_currency_of_the_book()
    {
      _myOrderBook = new OrderBook(TradeOrderType.Buy);
      var currencyPair = new CurrencyPair(ValidCurrencies.BitCoin, 1.00, ValidCurrencies.LiteCoin, 40.00);
      var tradeOrder1 = new TradeOrder(TradeOrderType.Buy, 0.5, currencyPair);
      _myOrderBook.AddOrder(tradeOrder1);
      Assert.AreEqual(ValidCurrencies.LiteCoin, _myOrderBook.OutgoingCurrency());
    }
  }
}