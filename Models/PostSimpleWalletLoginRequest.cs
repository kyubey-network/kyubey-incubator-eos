using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Andoromeda.Kyubey.Incubator.Models
{
    public class PostSimpleWalletLoginRequest
    {
        public string Protocol { get; set; }

        public string Version { get; set; }

        public long Timestamp { get; set; }

        public string Sign { get; set; }

        public string UUID { get; set; }

        public string Account { get; set; }

        public string Ref { get; set; }
    }
}
