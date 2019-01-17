using Microsoft.AspNetCore.Mvc;

namespace Andoromeda.Kyubey.Incubator.Models
{
    public class GetProjectListRequest : GetPagingRequest
    {
        [FromQuery(Name = "type")]
        public string type { get; set; } = "all";

        [FromQuery(Name = "status")]
        public string status { get; set; } = "all";

        [FromQuery(Name = "ranking")]
        public string ranking { get; set; } = "latest";
    }
}
