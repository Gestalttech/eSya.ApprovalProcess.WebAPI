using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ApprovalProcess.DO
{
    public class DO_Approver
    {
        public int UserID {  get; set; }
        public int ApprovalLevel { get; set; }
        public string? LoginDesc { get; set; }
        public string? ApprovalLevelDesc { get; set; }
    }
}
