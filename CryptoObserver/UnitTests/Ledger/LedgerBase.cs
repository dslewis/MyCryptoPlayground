using System;
using Core;

namespace UnitTests.Ledger
{
  public class LedgerBase
  {
    protected GainLossLedger FodderLedger;
    protected LedgerLine FodderLedgerLine;
    protected DateTime LongAgo;
    protected DateTime FarFromNow;

    public LedgerBase()
    {
      FodderLedger = new GainLossLedger();
      LongAgo = new DateTime(1900,1,1);
      FarFromNow = new DateTime(5000,1,1); 
    }
  }
}