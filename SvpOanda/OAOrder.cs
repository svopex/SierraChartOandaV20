using System.Collections.Generic;

namespace SvpOanda
{
    public class OAOrder
    {
        public long Id { get; set; }
        public double Price { get; set; }
        public double SL { get; set; }
        public double PT { get; set; }
        public string Instrument { get; set; }
        public long Units { get; set; }
    }

    public class OAOrders : List<OAOrder>
    {
    }
}
