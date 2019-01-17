using System;

namespace Andoromeda.Kyubey.Incubator.Models
{
    public class ProjectManifest
    {
        public string Id { get; set; }

        public string Cover { get; set; }

        public string Introduction { get; set; }

        public DateTime? Start_Time { get; set; }

        public DateTime DeadLine { get; set; }

        public decimal Target_Amount { get; set; }

        public decimal Target_Credits { get; set; }

        public int Number_Of_Supporters { get; set; }
    }
}
