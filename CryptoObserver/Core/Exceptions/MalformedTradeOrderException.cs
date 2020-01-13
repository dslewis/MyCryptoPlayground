using System;

namespace Core.Exceptions
{
  [Serializable]
  public class MalformedTradeOrderException : Exception
  {
    public MalformedTradeOrderException(string errorMessage) : base(errorMessage)
    {
    }
  }
}