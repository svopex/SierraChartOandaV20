using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OkonkwoOandaV20;
using OkonkwoOandaV20.Framework;
using OkonkwoOandaV20.TradeLibrary.DataTypes.Account;
using OkonkwoOandaV20.TradeLibrary.DataTypes.Communications;
using OkonkwoOandaV20.TradeLibrary.DataTypes.Communications.Requests;
using OkonkwoOandaV20.TradeLibrary.DataTypes.Communications.Requests.Order;
using OkonkwoOandaV20.TradeLibrary.DataTypes.Instrument;
using OkonkwoOandaV20.TradeLibrary.DataTypes.Order;
using OkonkwoOandaV20.TradeLibrary.DataTypes.Pricing;
using OkonkwoOandaV20.TradeLibrary.DataTypes.Trade;
using OkonkwoOandaV20.TradeLibrary.DataTypes.Transaction;

namespace SvpOanda
{
    public class SvpOandaV20
    {
        public static SvpOandaV20 Instance
        {
            get
            {
                if (svpOandaV20 == null)
                {
                    svpOandaV20 = new SvpOandaV20();
                }
                return svpOandaV20;
            }
        }
        private static SvpOandaV20 svpOandaV20 { get; set; }

        public async Task<Instrument> GetInstrument(string accountId, string instrument)
        {
            if (instruments == null)
            {
                instruments = await GetInstrumentEx(accountId);
            }
            return instruments.Find(x => x.name == instrument);
        }
        private List<Instrument> instruments;

        public async Task<Trade> GetMarketOrder(string accountId, long orderId)
        {
            Trade trade = await Rest20.GetTradeDetailsAsync(accountId, orderId);
            return trade;
        }

        public OAOrders GetMarketOrders(string accountId, string instrument)
        {
            Task<List<Trade>> callTask = Task.Run(() => Rest20.GetOpenTradeListAsync(accountId));
            callTask.Wait();

            OAOrders oaOrders = new OAOrders();
            foreach (Trade trade in callTask.Result.FindAll(x => x.instrument == instrument))
            {
                OAOrder oaOrder = new OAOrder();
                oaOrder.Id = trade.id;
                oaOrder.Units = trade.currentUnits;
                oaOrder.Instrument = trade.instrument;
                oaOrder.Price = trade.price;
                oaOrders.Add(oaOrder);
            }

            return oaOrders;
        }

        public async Task ModifyMarketOrder(string accountId, OAOrder order)
        {
            SetCredentials(accountId);

            PriceInformation priceInformation = new PriceInformation();
            priceInformation.instrument = await GetInstrument(accountId, order.Instrument);
            priceInformation.priceProperties = new List<string>();

            PatchExitOrdersRequest request = new PatchExitOrdersRequest();
            request.stopLoss = new StopLossDetails(instruments[0]);
            request.stopLoss.price = Math.Round(order.SL, priceInformation.instrument.displayPrecision);
            request.stopLoss.timeInForce = TimeInForce.GoodUntilCancelled;

            if (order.PT > 1) // != 0, kvuli zaokrouhlovani
            {
                request.takeProfit = new TakeProfitDetails(instruments[0]);
                request.takeProfit.price = Math.Round(order.PT, priceInformation.instrument.displayPrecision);
                request.takeProfit.timeInForce = TimeInForce.GoodUntilCancelled;
            }
            else
            {
                request.takeProfit = null;
            }

            try
            {
                TradePatchExitOrdersResponse tradePatchExitOrdersResponse = await Rest20.PatchTradeExitOrders(accountId, order.Id, request);
            }
            catch (Exception e)
            {
                if (e.Message.IndexOf("NO_SUCH_TRADE") > 0)
                {
                    return;
                }
                throw e;
            }
        }

        public async Task<long> CreateMarketOrder(string accountId, string instrument, long units)
        {
            MarketOrderRequest request = new MarketOrderRequest()
            {
                instrument = instrument,
                units = units,
            };
            OrderPostResponse response = await Rest20.PostOrderAsync(accountId, request);
            return response.orderFillTransaction.id;
        }

       
        public async Task<TradeCloseResponse> CloseOrder(String accountId, long orderId)
        {
            SetCredentials(accountId);
            try
            {
                TradeCloseResponse tradeCloseResponse = await Rest20.CloseTradeAsync(accountId, orderId);
                return tradeCloseResponse;
            }
            catch(Exception e)
            {
                if (e.Message.IndexOf("MARKET_ORDER_REJECT") > 0 && e.Message.IndexOf("TRADE_DOESNT_EXIST") > 0)
                {
                    return null;
                }
                throw e;
            }            
        }

        public async Task<double> GetActualPrice(string accountId, string instrument)
        {
            List<Price> price = await GetLastPrice(accountId, instrument);
            return (price[0].closeoutAsk + price[0].closeoutBid) / 2;
        }

        private async Task<List<Price>> GetLastPrice(string accountId, string instrument)
        {
            SetCredentials(accountId);
            List<Price> prices = await Rest20.GetPriceListAsync(accountId, new List<string>() { instrument }, new Dictionary<string, string>());
            return prices;
        }

        private void SetCredentials(string accountId)
        {
            Credentials.SetCredentials(EEnvironment.Practice, "ad6856656a565656d565656562440427-0f323b56565d65d65ffe58df0e6a6b93", accountId);
            //Credentials.SetCredentials(EEnvironment.Trade, "327c565d65d65d6d56d5d65d6d5d6d86-564c05656d565d6d56d5d656d3c61b06", accountId);
        }

        public async Task<List<Instrument>> GetInstrumentEx(string accountId)
        {
            List<Instrument> result = await Rest20.GetAccountInstrumentsAsync(accountId);
            return result;
        }

        public string GetOandaDateTime(DateTime dt)
        {
            return dt.ToUniversalTime().ToString("yyyy-MM-dd'T'HH:mm:ssZ");
        }

        public DateTime GetDateTime(string oandaDT)
        {
            return DateTime.Parse(oandaDT);
//            return dt.ToUniversalTime().ToString("yyyy-MM-dd'T'HH:mm:ssZ");
        }

        public async Task<List<ITransaction>> GetTransactions(string accountId, DateTime from)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("from", GetOandaDateTime(from));
            List<ITransaction> transactions = await Rest20.GetTransactionsByDateRangeAsync(accountId, parameters);
            return transactions;
        }
    }
}
