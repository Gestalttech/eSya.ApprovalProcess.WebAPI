using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ApprovalProcess.DO
{
    public class DO_ApprovalValues
    {
        public int BusinessKey { get; set; }
        public int FormId { get; set; }
        public int ApprovalLevel { get; set; }
        public decimal ValueFrom { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public decimal ValueTo { get; set; }
        public DateTime? EffectiveTill { get; set; }
        public bool ActiveStatus { get; set; }

    }
}
