using eSya.ApprovalProcess.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ApprovalProcess.IF
{
    public interface ICommonDataRepository
    {
        Task<List<DO_BusinessLocation>> GetBusinessKey();
        Task<List<DO_ApplicationCodes>> GetApplicationCodesByCodeTypeList(List<int> l_codeType);
    }
}
