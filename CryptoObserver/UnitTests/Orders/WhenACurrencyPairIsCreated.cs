using Core;
using Core.Enumerations;
using Core.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.Orders
{
  [TestClass]
  public class WhenACurrencyPairIsCreated
  {
    private CurrencyPair _myCurrencyPair;

    [TestMethod]
    public void It_should_convert_a_value_from_the_left_currency_to_the_right_one()
    {
      _myCurrencyPair = new CurrencyPair(ValidCurrencies.BitCoin, 1.00, ValidCurrencies.UsDollars, 500.00);
      Assert.AreEqual(1000.00, _myCurrencyPair.RightFromLeft(2.00));
    }

    [TestMethod]
    public void It_should_convert_a_value_from_the_right_currency_to_the_left_one()
    {
      _myCurrencyPair = new CurrencyPair(ValidCurrencies.BitCoin, 1.00, ValidCurrencies.UsDollars, 500.00);
      Assert.AreEqual(2.00, _myCurrencyPair.LeftFromRight(1000.00));
    }

    [TestMethod]
    public void It_should_not_allow_either_valuation_to_be_zero()
    {
      try
      {
        _myCurrencyPair = new CurrencyPair(ValidCurrencies.BitCoin, 0.00, ValidCurrencies.UsDollars, 100.00);
        Assert.Fail();
      }
      catch (MalformedCurrencyPairException e)
      {
        Assert.AreEqual("BitCoin has a value of 0.00, which isn't supported by CurrencyPair", e.Message);
      }
      try
      {
        _myCurrencyPair = new CurrencyPair(ValidCurrencies.BitCoin, 5.00, ValidCurrencies.UsDollars, 0.00);
        Assert.Fail();
      }
      catch (MalformedCurrencyPairException e)
      {
        Assert.AreEqual("UsDollars has a value of 0.00, which isn't supported by CurrencyPair", e.Message);
      }
    }

    [TestMethod]
    public void It_should_tell_you_what_currencies_and_values_it_was_created_with()
    {
      _myCurrencyPair = new CurrencyPair(ValidCurrencies.BitCoin, 2.00, ValidCurrencies.UsDollars, 975.00);
      Assert.AreEqual(ValidCurrencies.BitCoin, _myCurrencyPair.LeftCurrencySymbol);
      Assert.AreEqual(2.00, _myCurrencyPair.LeftCurrencyValue);
      Assert.AreEqual(ValidCurrencies.UsDollars, _myCurrencyPair.RightCurrencySymbol);
      Assert.AreEqual(975.00, _myCurrencyPair.RightCurrencyValue);
    }
  }
}