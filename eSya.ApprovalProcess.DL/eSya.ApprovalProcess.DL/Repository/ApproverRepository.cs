using eSya.ApprovalProcess.DL.Entities;
using eSya.ApprovalProcess.DO;
using eSya.ApprovalProcess.IF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ApprovalProcess.DL.Repository
{
    public class ApproverRepository: IApproverRepository
    {
        public async Task<List<DO_Forms>> GetApprovedFormsbyBusinesskey(int Businesskey)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {

                    var ds =await  db.GtEuusaps.Where(x =>x.BusinessKey== Businesskey && x.ActiveStatus)
                        .Join(db.GtEcfmfds.Where(w=>w.ActiveStatus),
                        uf => new { uf.FormId },
                        fm => new {fm.FormId},
                        (uf, fm) => new {uf,fm})
                        .Join(db.GtEcfmpas.Where(x => x.ParameterId == 3 && x.ActiveStatus),
                        f => f.fm.FormId,
                        p => p.FormId,
                        (f, p) => new { f, p })
                      .Select(x => new DO_Forms
                      {
                          FormID = x.f.uf.FormId,
                          FormName = x.f.fm.FormName,
                          FormCode = x.f.fm.FormCode
                      }).ToListAsync();
                    var Distinctforms = ds.GroupBy(x => x.FormID).Select(y => y.First());
                    return Distinctforms.ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<DO_Approver>> GetApproverUsresbyBusinesskey(int Businesskey, int FormID)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {

                    var ds = await db.GtEuusaps.Where(x => x.BusinessKey == Businesskey && x.FormId==FormID && x.ActiveStatus)
                        .Join(db.GtEuusms.Where(w => w.ActiveStatus),
                        u => new { u.UserId },
                        fu => new { fu.UserId },
                        (u, fu) => new { u, fu })
                        .Join(db.GtEcapcds.Where(x => x.ActiveStatus),
                        a =>new { a.u.ApprovalLevel },
                        p =>new { ApprovalLevel=p.ApplicationCode },
                        (a, p) => new { a, p })
                      .Select(x => new DO_Approver
                      {
                         UserID=x.a.u.UserId,
                         LoginDesc=x.a.fu.LoginDesc,
                         ApprovalLevel = x.a.u.ApprovalLevel,
                         ApprovalLevelDesc=x.p.CodeDesc
                      }).ToListAsync();
                    return ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
