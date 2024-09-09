using eSya.ApprovalProcess.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ApprovalProcess.IF
{
    public interface IProcessRepository
    {
        #region Approval Level Based
        Task<List<DO_Forms>> GetFormsForApproval();
        Task<DO_ApprovalTypes> GetApprovalTypebyFormID(int businesskey, int formId);
        Task<List<DO_ApprovalLevels>> GetApprovalLevelsbyCodeType(int codetype , int businesskey, int formId, int approvaltype);
        Task<DO_ReturnParameter> InsertOrUpdateApprovalLevel(DO_ApprovalTypes obj);
        #endregion
        #region Approval Value Based
        Task<DO_ReturnParameter> InsertOrUpdateApprovalValueBased(DO_ApprovalTypes obj);
        Task<List<DO_ApprovalValues>> GetApprovalValuesbyFormID(int businesskey, int formId);
        #endregion
    }
}
