using System.ServiceProcess;
using CoinBaseLedgerManager;

namespace LedgerManager
{
  internal static class Program
  {
    private static void Main()
    {
      var servicesToRun = new ServiceBase[]
      {
        new CoinBaseLedgerManagerService()
      };
      ServiceBase.Run(servicesToRun);
    }
  }
}