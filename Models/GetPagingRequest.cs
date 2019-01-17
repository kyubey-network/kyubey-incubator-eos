using Microsoft.AspNetCore.Mvc;

namespace Andoromeda.Kyubey.Incubator.Models
{
    public class GetPagingRequest : GetBaseRequest
    {
        [FromQuery(Name = "skip")]
        public int Skip { get; set; } = 0;

        [FromQuery(Name = "take")]
        public int Take { get; set; } = 50;
    }
}
