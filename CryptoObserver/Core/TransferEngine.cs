using System;
using System.Linq;
using Core.ApiComponents;
using Core.Enumerations;
using Newtonsoft.Json;

namespace Core
{
  public class TransferEngine
  {
    public static GainLossLedger CoinbaseLedgerUpdater(string transferJson, GainLossLedger existingLedger)
    {
      var dataSet = JsonConvert.DeserializeObject<CoinbaseTransferResult>(transferJson, new JsonSerializerSettings {MissingMemberHandling = MissingMemberHandling.Error});

      foreach (var ledgerLine in from nestedTransfer in dataSet.Transfers let pair = new CurrencyPair(ValidCurrencies.BitCoin,
        Convert.ToDouble(nestedTransfer.Transfer.Btc["amount"]),
        ValidCurrencies.UsDollars,
        Convert.ToDouble(nestedTransfer.Transfer.Total["amount"])) select new LedgerLine(nestedTransfer.Transfer.Id, BuyOrSell(nestedTransfer.Transfer.Type),
          Convert.ToDouble(nestedTransfer.Transfer.Btc["amount"]),
          pair,
          DateTime.Parse(nestedTransfer.Transfer.Created_At)))
      {
        existingLedger.UpsertLine(ledgerLine);
      }
      return existingLedger;
    }

    private static TradeOrderType BuyOrSell(string type)
    {
      switch (type)
      {
        case "Buy":
          return TradeOrderType.Buy;
        case "Sell":
          return TradeOrderType.Sell;
      }
      throw new InvalidOperationException(String.Format("{0} received from Coinbase is not a valid order type", type));
    }
  }
}