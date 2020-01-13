using System;

namespace Core.Exceptions
{
  [Serializable]
  public class OrderBookOverrunException : Exception
  {
    public OrderBookOverrunException(string errorMessage) : base(errorMessage)
    {
    }
  }
}