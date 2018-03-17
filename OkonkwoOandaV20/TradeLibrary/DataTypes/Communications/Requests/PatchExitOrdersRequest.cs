using Newtonsoft.Json;
using OkonkwoOandaV20.TradeLibrary.DataTypes.Transaction;

namespace OkonkwoOandaV20.TradeLibrary.DataTypes.Communications.Requests
{
   public class PatchExitOrdersRequest : Request
   {
      [JsonProperty(NullValueHandling = NullValueHandling.Include)] // !pokud je takeProfit == null, zrusi se a to potrebuju!!!
      public TakeProfitDetails takeProfit { get; set; }

      public StopLossDetails stopLoss { get; set; }

      public TrailingStopLossDetails trailingStopLoss { get; set; }
   }
}
