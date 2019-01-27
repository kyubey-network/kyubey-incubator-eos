using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Andoromeda.Kyubey.Incubator.Models
{
    public class GetIncubatorInfoResponse
    {
        public decimal CurrentPrice { get; set; }

        public decimal? Balance { get; set; }

        public bool IsFavorite { get; set; }

        public decimal TotalSupply { get; set; }

        public decimal CurrentRaised { get; set; }

        public int SupporterCount { get; set; }

        public int RemainingDay { get; set; }

        public decimal Target { get; set; }

        public string Contract { get; set; }

        public string Protocol { get; set; }
    }
}
