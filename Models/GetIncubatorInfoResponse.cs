using System;

namespace Andoromeda.Kyubey.Incubator.Models
{
    public class GetIncubatorInfoResponse
    {
        public decimal CurrentPrice { get; set; }

        public decimal? EOSBalance { get; set; }

        public decimal? TokenBalance { get; set; }

        public bool IsFavorite { get; set; }

        public decimal TotalSupply { get; set; }

        public decimal CurrentRaised { get; set; }

        public int SupporterCount { get; set; }

        public int RemainingDay { get; set; }

        public DateTime? BeginTime { get; set; }

        public DateTime DeadLine { get; set; }

        public decimal Target { get; set; }

        public string Contract { get; set; }

        public string Protocol { get; set; }

        public string WhitePaper { get; set; }

        public string BuyMemo { get; set; }
    }
}
