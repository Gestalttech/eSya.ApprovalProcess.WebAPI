using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ApprovalProcess.DO
{
    public class DO_ApprovalLevels
    {
        public int BusinessKey { get; set; }
        public int FormId { get; set; }
        public int ApprovalLevel { get; set; }
        public bool ActiveStatus { get; set; }
        public int UserID { get; set; }
        public string TerminalID { get; set; }
        public string? ApprovalLevelDesc { get; set; }
    }
}
