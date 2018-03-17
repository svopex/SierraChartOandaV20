﻿using OkonkwoOandaV20.TradeLibrary.DataTypes.Transaction;
using System.Collections.Generic;

namespace OkonkwoOandaV20.TradeLibrary.DataTypes.Communications
{
   public class OrderClientExtensionsModifyResponse : Response
   {
      public OrderClientExtensionsModifyTransaction orderClientExtensionsModifyTransaction;
      public List<long> relatedTransactionIDs;
      public long lastTransactionID;
   }
}
