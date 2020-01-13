using System;
using System.Configuration;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using Core;
using Core.Enumerations;
using Core.Security;
using Newtonsoft.Json;

namespace CommandLineManager
{
  internal class Program
  {
    private static GainLossLedger _myLedger;
    private static string _coinbaseApiKey;
    private static string _coinbaseApiSecret;
    private static string _theKey;

    public static void Main(string[] args)
    {
      var running = true;
      var keyDirectory = ConfigurationManager.AppSettings.Get("KeyDirectory");
      var coinbaseApiKeyFileManager = new FileManager(Path.Combine(keyDirectory, "CoinBaseApiKey.txt"));
      var coinbaseApiSecretManager = new FileManager(Path.Combine(keyDirectory, "CoinBaseApiSecret.txt"));
      var ledgerFileManager =
        new FileManager(Path.Combine(ConfigurationManager.AppSettings.Get("LedgerDirectory"), "Ledger.txt"));

      while (true)
      {
        try
        {
          Console.WriteLine("What's the key?");
          _theKey = Console.ReadLine();
          Console.Clear();
          if (string.IsNullOrWhiteSpace(_theKey))
          {
            Console.WriteLine("Jeez.  At least enter something");
          }
          else
          {
            _coinbaseApiKey = EncryptionManager.Decrypt(coinbaseApiKeyFileManager.Load(), _theKey);
            _coinbaseApiSecret = EncryptionManager.Decrypt(coinbaseApiSecretManager.Load(), _theKey);
            _myLedger = ledgerFileManager.Exists
              ? JsonConvert.DeserializeObject<GainLossLedger>(
                EncryptionManager.Decrypt(ledgerFileManager.Load(), _theKey),
                new JsonSerializerSettings {MissingMemberHandling = MissingMemberHandling.Error})
              : new GainLossLedger();
            break;
          }
          Thread.Sleep(2000);
          Console.Clear();
        }
        catch (CryptographicException)
        {
          Console.WriteLine("Sorry.  I was unable to decrypt anything with the key you gave me.  Try again.");
          Thread.Sleep(2000);
          Console.Clear();
        }
      }

      do
      {
        var choice = ShowMenu();
        switch (choice)
        {
          case 1:
            _myLedger = AddTransaction(_myLedger);
            break;
          case 2:
            CalculateGains(_myLedger);
            break;
          case 3:
            PrintBalance(_myLedger);
            break;
          case 4:
            Console.WriteLine(_coinbaseApiKey);
            break;
          case 5:
            coinbaseApiKeyFileManager.Save(() => EncryptionManager.Encrypt(Console.ReadLine(), _theKey));
            break;
          case 6:
            Console.WriteLine(_coinbaseApiSecret);
            break;
          case 7:
            coinbaseApiSecretManager.Save(() => EncryptionManager.Encrypt(Console.ReadLine(), _theKey));
            break;
          case 8:
            ledgerFileManager.Save(() => EncryptionManager.Encrypt(JsonConvert.SerializeObject(_myLedger), _theKey));
            running = false;
            break;
          default:
            Console.WriteLine("{0} Isn't a valid choice");
            break;
        }
        Thread.Sleep(2000);
        Console.Clear();
      } while (running);
    }

    private static GainLossLedger AddTransaction(GainLossLedger ledger)
    {
      TradeOrderType type;
      ValidCurrencies coin;
      Console.WriteLine("Enter a transaction as Id, B/S,amount,B/L,UsDollar value");
      var readLine = Console.ReadLine();
      if (readLine == null) return ledger;
      var trans = readLine.Split(',');
      switch (trans[1])
      {
        case "B":
          type = TradeOrderType.Buy;
          break;
        case "S":
          type = TradeOrderType.Sell;
          break;
        default:
          throw new InvalidOperationException();
      }

      switch (trans[3])
      {
        case "B":
          coin = ValidCurrencies.BitCoin;
          break;
        case "L":
          coin = ValidCurrencies.LiteCoin;
          break;
        default:
          throw new InvalidOperationException();
      }

      ledger.UpsertLine(new LedgerLine(trans[0], type, Convert.ToDouble(trans[1]),
        new CurrencyPair(coin, 1.00, ValidCurrencies.UsDollars, Convert.ToDouble(trans[3])), DateTime.Now));

      return ledger;
    }

    private static void CalculateGains(GainLossLedger ledger)
    {
      var start = new DateTime(1900, 1, 1);
      var end = new DateTime(2900, 1, 1);
      var calc = ledger.CalculateGainOrLossForLedger(start, end);
      Console.WriteLine("Your current exposure is {0} dollars in short term gains and {1} dollars in long term gains",
        calc.ShortTermGain, calc.LongTermGain);
      Thread.Sleep(1000);
    }

    private static void PrintBalance(GainLossLedger ledger)
    {
      Console.WriteLine("Your current balance is {0} BitCoin and {1} LiteCoin",
        ledger.CurrencyBalance(ValidCurrencies.BitCoin), ledger.CurrencyBalance(ValidCurrencies.LiteCoin));
    }

    private static int ShowMenu()
    {
      Console.WriteLine("1. Record a manual transaction");
      Console.WriteLine("2. Check your Capital Gains Exposure");
      Console.WriteLine("3. Check your balances");
      Console.WriteLine("4. Show current Coinbase API Key");
      Console.WriteLine("5. Set Coinbase API Key");
      Console.WriteLine("6. Show current Coinbase API Secret");
      Console.WriteLine("7. Set Coinbase API Secret");
      Console.WriteLine("8. Quit");
      return Convert.ToInt32(Console.ReadLine());
    }
  }
}