using Microsoft.AspNetCore.Mvc;

namespace Andoromeda.Kyubey.Incubator.Models
{
    public class GetBaseRequest
    {
        [FromRoute(Name = "lang")]
        public string Lang { get; set; }
    }
}
