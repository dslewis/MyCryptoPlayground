using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.ServiceProcess;
using System.Text;
using System.Timers;
using Core;
using Core.ApiComponents;
using Core.Security;
using Newtonsoft.Json;

namespace CoinBaseLedgerManager
{
  public partial class CoinBaseLedgerManagerService : ServiceBase
  {
    public CoinBaseLedgerManagerService()
    {
      InitializeComponent();
    }

    public string FilePath { get; set; }

    public GainLossLedger CurrentLedger { get; set; }
    public string CoinBaseApiKey { get; set; }
    public string CoinBaseApiSecret { get; set; }
    public string TheKey { get; set; }
    public bool DecryptSucceeded { get; set; }
    public FileManager LedgerFileManager { get; set; }

    protected override void OnStart(string[] args)
    {
      DecryptSucceeded = false;
      if (args == null || !args.Any())
      {
        EventLog.WriteEntry("No key was provided on Startup", EventLogEntryType.Error);
        Stop();
        return;
      }
      TheKey = args[0];
      EventLog.WriteEntry("Ledger Manager Service", "Service Started");

      var keyDirectory = ConfigurationManager.AppSettings.Get("KeyDirectory");
      var apiKeyFileManager = new FileManager(Path.Combine(keyDirectory, "CoinBaseApiKey.txt"));
      var apiSecretFileManager = new FileManager(Path.Combine(keyDirectory, "CoinBaseApiSecret.txt"));
      LedgerFileManager =
        new FileManager(Path.Combine(ConfigurationManager.AppSettings.Get("LedgerDirectory"), "Ledger.txt"));
      try
      {
        CoinBaseApiKey = EncryptionManager.Decrypt(apiKeyFileManager.Load(), TheKey);
        CoinBaseApiSecret = EncryptionManager.Decrypt(apiSecretFileManager.Load(), TheKey);
        CurrentLedger = LedgerFileManager.Exists
          ? JsonConvert.DeserializeObject<GainLossLedger>(
            EncryptionManager.Decrypt(LedgerFileManager.Load(), args[0]),
            new JsonSerializerSettings {MissingMemberHandling = MissingMemberHandling.Error})
          : new GainLossLedger();
        DecryptSucceeded = true;
      }
      catch (CryptographicException)
      {
        EventLog.WriteEntry("Unable to decrypt service data with the key provided.", EventLogEntryType.Error);
        Stop();
      }

      var timer = new Timer {Interval = string.IsNullOrWhiteSpace(args[1]) ? 300000 : Convert.ToInt32(args[1])};
      timer.Elapsed += Ontimer;
      timer.Start();
    }

    protected override void OnStop()
    {
      if (DecryptSucceeded)
      {
        LedgerFileManager.Save(() => EncryptionManager.Encrypt(JsonConvert.SerializeObject(CurrentLedger), TheKey));
      }
      EventLog.WriteEntry("Ledger Manager Service", "Service Stopped");
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
    private void Ontimer(object sender, ElapsedEventArgs e)
    {
      var responseStream =
        AuthorizedCoinBaseRequest.GenerateRequest("https://coinbase.com/api/v1/transfers", "GET", CoinBaseApiKey,
          CoinBaseApiSecret, DateTime.Now.Ticks.ToString(), "").GetResponse().GetResponseStream();
      if (responseStream == null)
      {
        EventLog.WriteEntry("Couldn't get response stream from CoinBase", EventLogEntryType.Warning);
      }
      else
      {
        var reader = new StreamReader(responseStream, Encoding.GetEncoding(1252));
        CurrentLedger = TransferEngine.CoinbaseLedgerUpdater(reader.ReadToEnd(), CurrentLedger);
        reader.Close();
        responseStream.Close();
        EventLog.WriteEntry("Updated Ledger from Coinbase");
      }
      LedgerFileManager.Save(() => EncryptionManager.Encrypt(JsonConvert.SerializeObject(CurrentLedger), TheKey));
    }
  }
}