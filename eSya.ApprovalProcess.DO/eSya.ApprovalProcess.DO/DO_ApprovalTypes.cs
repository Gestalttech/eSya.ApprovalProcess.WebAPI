using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ApprovalProcess.DO
{
    public class DO_ApprovalTypes
    {
        public int BusinessKey { get; set; }
        public int FormId { get; set; }
        public int ApprovalType { get; set; }
        public bool ActiveStatus { get; set; }
        public int UserID { get; set; }
        public string TerminalID { get; set; }
        public List<DO_ApprovalLevels>? lst_ApprovalLevels { get; set; }
        public List<DO_ApprovalValues>? lst_ApprovalValues { get; set; }
        
    }
}
