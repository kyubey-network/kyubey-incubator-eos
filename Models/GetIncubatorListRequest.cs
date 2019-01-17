using Microsoft.AspNetCore.Mvc;

namespace Andoromeda.Kyubey.Incubator.Models
{
    public class GetIncubatorListRequest : GetPagingRequest
    {
        [FromQuery(Name = "type")]
        public string Type { get; set; } = "all";

        [FromQuery(Name = "status")]
        public string Status { get; set; } = "all";

        [FromQuery(Name = "ranking")]
        public string Ranking { get; set; } = "latest";
    }
}
