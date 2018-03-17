using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SierraChartServiceClient
{
    public class InstrumentConversion
    {
        public const string SierraChartAccountId = "101-004-1010129-001"; // DEMO
        //public const int SierraChartAccountId =  "001-004-424879-001"; // LIVE

        public string SierraInstrumentName { get; set; }
        public string OandaInstrumentName { get; set; }
        public double SL { get; set; }
        public double Compensation { get; set; }

        public static List<InstrumentConversion> InstrumentConversions = new List<InstrumentConversion>()
        {
            new InstrumentConversion() { SierraInstrumentName = "NQ", OandaInstrumentName = "NAS100_USD", SL = 5, Compensation = 0.8 },
            new InstrumentConversion() { SierraInstrumentName = "ES", OandaInstrumentName = "SPX500_USD", SL = 2.5, Compensation = 0.5},
            new InstrumentConversion() { SierraInstrumentName = "TF", OandaInstrumentName = "US2000_USD", SL = 2, Compensation = 0.2 },
            new InstrumentConversion() { SierraInstrumentName = "YM", OandaInstrumentName = "US30_USD", SL = 20, Compensation = 2 }
        };
    }
}
