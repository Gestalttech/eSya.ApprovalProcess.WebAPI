using eSya.ApprovalProcess.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ApprovalProcess.IF
{
    public interface IApproverRepository
    {
        Task<List<DO_Forms>> GetApprovedFormsbyBusinesskey(int Businesskey);
        Task<List<DO_Approver>> GetApproverUsresbyBusinesskey(int Businesskey, int FormID);
    }
}
