using System;
using System.Collections.Generic;
using System.Linq;
using Core.Enumerations;

namespace Core
{
  public class GainLossLedger
  {
    private DateTime _periodEnd;
    private DateTime _periodStart;

    public GainLossLedger()
    {
      Transactions = new List<LedgerLine>();
    }

    public List<LedgerLine> Transactions { get; private set; }

    public CapitalGain CalculateGainOrLoss(ValidCurrencies currency, DateTime periodStart, DateTime periodEnd)
    {
      OrderByDate();
      var totalGain = new CapitalGain();
      LedgerLine fractionBuyAmount = null;
      var buys = QueueTransactions(TradeOrderType.Buy, currency);
      var sells = QueueTransactions(TradeOrderType.Sell, currency);
      _periodStart = periodStart;
      _periodEnd = periodEnd;

      while (sells.Count > 0)
      {
        var sell = sells.Dequeue();
        double remainingSaleAmount;

        if (fractionBuyAmount != null && fractionBuyAmount.Amount <= sell.Amount)
        {
          remainingSaleAmount = sell.Amount - fractionBuyAmount.Amount;
          totalGain = ComputeGainFromABuy(totalGain, fractionBuyAmount.Amount, fractionBuyAmount.TradeCurrencyPair,
            fractionBuyAmount.WhenTransaction, fractionBuyAmount.Amount, sell.TradeCurrencyPair, sell.WhenTransaction);
        }
        else if (fractionBuyAmount != null && fractionBuyAmount.Amount >= sell.Amount)
        {
          totalGain = ComputeGainFromABuy(totalGain, sell.Amount, fractionBuyAmount.TradeCurrencyPair,
            fractionBuyAmount.WhenTransaction, sell.Amount, sell.TradeCurrencyPair, sell.WhenTransaction);
          fractionBuyAmount = new LedgerLine("temp", TradeOrderType.Buy, fractionBuyAmount.Amount - sell.Amount,
            fractionBuyAmount.TradeCurrencyPair, DateTime.Now);
          remainingSaleAmount = 0.00;
        }
        else
        {
          remainingSaleAmount = sell.Amount;
        }

        while (remainingSaleAmount > 0.00)
        {
          var individualBuy = buys.Dequeue();
          if (remainingSaleAmount >= individualBuy.Amount)
          {
            totalGain = ComputeGainFromABuy(totalGain, individualBuy.Amount, individualBuy.TradeCurrencyPair,
              individualBuy.WhenTransaction, individualBuy.Amount, sell.TradeCurrencyPair, sell.WhenTransaction);
            remainingSaleAmount -= individualBuy.Amount;
          }
          else
          {
            fractionBuyAmount = new LedgerLine("temp", TradeOrderType.Buy, individualBuy.Amount - remainingSaleAmount,
              individualBuy.TradeCurrencyPair, individualBuy.WhenTransaction);
            totalGain = ComputeGainFromABuy(totalGain, remainingSaleAmount, individualBuy.TradeCurrencyPair,
              individualBuy.WhenTransaction, remainingSaleAmount, sell.TradeCurrencyPair, sell.WhenTransaction);
            remainingSaleAmount = 0.00;
          }
        }
      }
      return totalGain;
    }

    public CapitalGain CalculateGainOrLossForLedger(DateTime periodStart, DateTime periodEnd)
    {
      _periodStart = periodStart;
      _periodEnd = periodEnd;
      var currencies = Enum.GetValues(typeof (ValidCurrencies));
      var shortTotal =
        currencies.Cast<ValidCurrencies>()
          .Sum(currency => CalculateGainOrLoss(currency, periodStart, periodEnd).ShortTermGain);
      var longTotal =
        currencies.Cast<ValidCurrencies>()
          .Sum(currency => CalculateGainOrLoss(currency, periodStart, periodEnd).LongTermGain);
      return new CapitalGain {ShortTermGain = shortTotal, LongTermGain = longTotal};
    }

    public double CurrencyBalance(ValidCurrencies requestedCoin)
    {
      var balance = 0.00;
      foreach (
        var transaction in
          Transactions.Where(transaction => transaction.TradeCurrencyPair.LeftCurrencySymbol == requestedCoin))
      {
        if (transaction.TradeType == TradeOrderType.Buy)
        {
          balance += transaction.Amount;
        }
        else
        {
          balance -= transaction.Amount;
        }
      }

      return balance;
    }

    public void OrderByDate()
    {
      Transactions = Transactions.OrderBy(t => t.WhenTransaction).ToList();
    }

    public void UpsertLine(LedgerLine lineItem)
    {
      var transactionExists = false;
      foreach (var transaction in Transactions.Where(transaction => transaction.Id == lineItem.Id))
      {
        if (transaction.TradeType != lineItem.TradeType)
        {
          throw new InvalidOperationException("You cannot change the trade order type of an existing transaction.");
        }
        transaction.Amount = lineItem.Amount;
        transaction.TradeCurrencyPair = lineItem.TradeCurrencyPair;
        transaction.WhenTransaction = lineItem.WhenTransaction;
        transactionExists = true;
      }
      if (!transactionExists)
      {
        Transactions.Add(lineItem);
      }
      OrderByDate();
    }

    private CapitalGain ComputeGainFromABuy(CapitalGain tally, double buyAmount, CurrencyPair buyExchange,
      DateTime buyTime, double saleAmount, CurrencyPair saleExchange, DateTime sellTime)
    {
      var capGain = (saleAmount*saleExchange.RightCurrencyValue) - (buyAmount*buyExchange.RightCurrencyValue);
      if ((_periodStart > sellTime) || (sellTime > _periodEnd)) return tally;
      if (buyTime < sellTime.Subtract(new TimeSpan(730, 0, 0, 0)))
      {
        tally.LongTermGain += capGain;
      }
      else
      {
        tally.ShortTermGain += capGain;
      }
      return tally;
    }

    private Queue<LedgerLine> QueueTransactions(TradeOrderType type, ValidCurrencies currency)
    {
      var trans = new Queue<LedgerLine>();
      foreach (
        var transaction in
          Transactions.Where(
            transaction => transaction.TradeType == type && transaction.TradeCurrencyPair.LeftCurrencySymbol == currency)
        )
      {
        trans.Enqueue(transaction);
      }
      return trans;
    }
  }
}