using OkonkwoOandaV20;
using OkonkwoOandaV20.TradeLibrary.DataTypes.Account;
using OkonkwoOandaV20.TradeLibrary.DataTypes.Communications;
using OkonkwoOandaV20.TradeLibrary.DataTypes.Transaction;
using SvpOanda;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SvpTest
{
    class Program
    {
        static void Main(string[] args)
        {
            SvpOandaV20 test = new SvpOandaV20();
            Task<List<ITransaction>> callTask = test.GetTransactions("101-004-1010129-001", DateTime.Now.Date);
            callTask.Wait();
            List<OrderFillTransaction> orderFillTransactions = 
                callTask
                .Result
                .Where(x => x is OrderFillTransaction && ((OrderFillTransaction)x).tradesClosed != null)
                .Cast<OrderFillTransaction>()
                .ToList();

            using (StreamWriter writer = new StreamWriter("c:\\log\\transactions.csv", false))
            {
                foreach (OrderFillTransaction tr in orderFillTransactions)
                {
                    writer.WriteLine(tr.instrument + "," + SvpOandaV20.Instance.GetDateTime(tr.time).ToString() + "," + -tr.units + "," + tr.pl.ToString());
                }
            }
        }
    }
}
