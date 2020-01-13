using System.Collections.Generic;

namespace Core.ApiComponents
{
  public class BtcEOrdersResult
  {
    public IList<string[]> Asks { get; set; }
    public IList<string[]> Bids { get; set; } 
  }
}