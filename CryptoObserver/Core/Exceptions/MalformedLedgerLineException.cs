using System;

namespace Core.Exceptions
{
  [Serializable]
  public class MalformedLedgerLineException : Exception
  {
    public MalformedLedgerLineException(string errorMessage) : base(errorMessage)
    {
    }
  }
}