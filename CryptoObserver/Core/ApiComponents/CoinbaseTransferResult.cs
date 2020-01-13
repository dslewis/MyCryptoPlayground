using System.Collections.Generic;

namespace Core.ApiComponents
{
  public class CoinbaseTransferResult
  {
    public string Current_Page { get; set; }
    public string Num_Pages { get; set; }
    public string Total_Count { get; set; }
    public NestedTransfer[] Transfers { get; set; }
  }

  public class Transfer
  {
    public Transfer()
    {
    }

    public string Id { get; set; }
    public string Type { get; set; }
    public string _Type { get; set; }
    public string Code { get; set; }
    public string Created_At { get; set; }
    public Fees Fees { get; set; }
    public string Status { get; set; }
    public string Payout_Date { get; set; }
    public IDictionary<string, string> Btc { get; set; }
    public IDictionary<string, string> Subtotal { get; set; }
    public IDictionary<string, string> Total { get; set; }
    public string Transaction_Id { get; set; }
    public string Description { get; set; }
    public string Detailed_Status { get; set; }
    public string Account { get; set; }
    public PaymentMethod Payment_Method { get; set; }
  }

  public class Fees
  {
    public IDictionary<string, string> Bank;
    public IDictionary<string, string> Coinbase;
  }

  public class PaymentMethod
  {
    public string Can_Buy;
    public string Can_Sell;
    public string Currency;
    public string Id;
    public string Name;
    public string Type;
  }

  public class NestedTransfer
  {
    public NestedTransfer()
    {
    }

    public Transfer Transfer { get; set; }
  }
}