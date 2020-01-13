using Core;
using Core.Enumerations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTests.Framework;

namespace UnitTests.Forecasting
{
  [TestClass]
  public class WhenIGetThePathsThroughAGroupOfOrderBooks
  {
    public OrderBook BuyBitCoinWithLiteCoin;
    public OrderBook BuyDollarsWithBitcoin;
    public OrderBook BuyLiteCoinWithDollars;
    public OrderBook SellBitCoinForLiteCoin;
    public OrderBook SellDollarsForBitCoin;
    public OrderBook SellLiteCoinForDollars;

    public WhenIGetThePathsThroughAGroupOfOrderBooks()
    {
      SellBitCoinForLiteCoin =
        FactoryMethods.OrderBookTestFiller(
          new TradeOrder(TradeOrderType.Sell, 1.0,
            new CurrencyPair(ValidCurrencies.BitCoin, 1.0, ValidCurrencies.LiteCoin, 10.0)), 5, 0.5);
      SellLiteCoinForDollars =
        FactoryMethods.OrderBookTestFiller(
          new TradeOrder(TradeOrderType.Sell, 1.0,
            new CurrencyPair(ValidCurrencies.LiteCoin, 1.0, ValidCurrencies.UsDollars, 40.0)), 5, 2.0);
      SellDollarsForBitCoin =
        FactoryMethods.OrderBookTestFiller(
          new TradeOrder(TradeOrderType.Sell, 100.00,
            new CurrencyPair(ValidCurrencies.UsDollars, 400.0, ValidCurrencies.BitCoin, 1.0)), 5, 50.0);
      BuyBitCoinWithLiteCoin =
        FactoryMethods.OrderBookTestFiller(
          new TradeOrder(TradeOrderType.Buy, 1.0,
            new CurrencyPair(ValidCurrencies.BitCoin, 1.0, ValidCurrencies.LiteCoin, 10.0)), 5, 0.5);
      BuyLiteCoinWithDollars =
        FactoryMethods.OrderBookTestFiller(
          new TradeOrder(TradeOrderType.Buy, 1.0,
            new CurrencyPair(ValidCurrencies.LiteCoin, 1.0, ValidCurrencies.UsDollars, 40.0)), 5, 2.0);
      BuyDollarsWithBitcoin =
        FactoryMethods.OrderBookTestFiller(
          new TradeOrder(TradeOrderType.Buy, 100.00,
            new CurrencyPair(ValidCurrencies.UsDollars, 400.0, ValidCurrencies.BitCoin, 1.0)), 5, 50.0);
    }

    [TestMethod]
    public void It_should_have_the_correct_count_in_my_order_books()
    {
      Assert.AreEqual(5, BuyBitCoinWithLiteCoin.TradeCount());
      Assert.AreEqual(5, SellLiteCoinForDollars.TradeCount());
    }
  }
}