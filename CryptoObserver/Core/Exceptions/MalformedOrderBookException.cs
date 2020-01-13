using System;

namespace Core.Exceptions
{
  [Serializable]
  public class MalformedOrderBookException : Exception
  {
    public MalformedOrderBookException(string errorMessage) : base(errorMessage)
    {
    }
  }
}