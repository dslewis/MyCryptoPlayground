using System;

namespace Core.Exceptions
{
  [Serializable]
  public class MalformedCalculationException : Exception
  {
    public MalformedCalculationException(string errorMessage) : base(errorMessage)
    {
    }
  }
}