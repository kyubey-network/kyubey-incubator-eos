using System;

namespace Andoromeda.Kyubey.Incubator.Models
{
    public class GetIncubatorListResponse
    {
        public string Id { get; set; }

        public string Cover { get; set; }

        public string Introduction { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime DeadLine { get; set; }

        public decimal TargetAmount { get; set; }

        public decimal TargetCredits { get; set; }

        public int NumberOfSupporters { get; set; }
    }

    public class GetIncubatorQuantityResponse
    {
        public int Total { get; set; }
    }
}
