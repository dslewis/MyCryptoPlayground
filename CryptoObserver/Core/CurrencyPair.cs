using System;
using Core.Enumerations;
using Core.Exceptions;

namespace Core
{
  public class CurrencyPair
  {
    public CurrencyPair(ValidCurrencies leftCurrencySymbol, double leftCurrencyValue,
      ValidCurrencies rightCurrencySymbol, double rightCurrencyValue)
    {
      if (Math.Abs(leftCurrencyValue) < .000001)
      {
        var errorDescription = String.Format(
          "{0} has a value of 0.00, which isn't supported by CurrencyPair",
          leftCurrencySymbol);
        throw new MalformedCurrencyPairException(errorDescription);
      }
      if (Math.Abs(rightCurrencyValue) < .000001)
      {
        var errorDescription = String.Format(
          "{0} has a value of 0.00, which isn't supported by CurrencyPair",
          rightCurrencySymbol);
        throw new MalformedCurrencyPairException(errorDescription);
      }
      LeftCurrencySymbol = leftCurrencySymbol;
      LeftCurrencyValue = leftCurrencyValue;
      RightCurrencySymbol = rightCurrencySymbol;
      RightCurrencyValue = rightCurrencyValue;
    }

    public double RightCurrencyValue { get; private set; }

    public double LeftCurrencyValue { get; private set; }

    public ValidCurrencies LeftCurrencySymbol { get; private set; }

    public ValidCurrencies RightCurrencySymbol { get; private set; }


    public double LeftFromRight(double rightCurAmt)
    {
      return rightCurAmt*(LeftCurrencyValue/RightCurrencyValue);
    }

    public double RightFromLeft(double leftCurAmt)
    {
      return leftCurAmt/(LeftCurrencyValue/RightCurrencyValue);
    }
  }
}