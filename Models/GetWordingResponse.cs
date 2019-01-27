using System.Collections.Generic;

namespace Andoromeda.Kyubey.Incubator.Models
{
    public class GetWordingResponse
    {
        public string TokenId { get; set; }

        public List<string> Sliders { get; set; }

        public string Description { get; set; }

        public string Detail { get; set; }

        public List<TokenIncubatorUpdateModel> Updates { get; set; }
    }
}
