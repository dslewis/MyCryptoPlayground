using System;

namespace Core.Exceptions
{
  [Serializable]
  public class MalformedCurrencyPairException : Exception
  {
    public MalformedCurrencyPairException(string errorMessage) : base(errorMessage)
    {
    }
  }
}