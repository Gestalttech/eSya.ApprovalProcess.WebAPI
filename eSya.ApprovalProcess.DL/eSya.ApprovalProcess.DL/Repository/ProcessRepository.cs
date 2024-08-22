using eSya.ApprovalProcess.DL.Entities;
using eSya.ApprovalProcess.DO;
using eSya.ApprovalProcess.IF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ApprovalProcess.DL.Repository
{
    public class ProcessRepository : IProcessRepository
    {
        private readonly IStringLocalizer<ProcessRepository> _localizer;
        public ProcessRepository(IStringLocalizer<ProcessRepository> localizer)
        {
            _localizer = localizer;
        }
        #region Approval Level Based
        public async Task<List<DO_Forms>> GetFormsForApproval()
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {

                    var ds = await db.GtEcfmfds.Where(x => x.ActiveStatus)
                        .Join(db.GtEcfmpas.Where(x => x.ParameterId == 3 && x.ActiveStatus),
                        f => f.FormId,
                        p => p.FormId,
                        (f, p) => new { f, p })
                      .Select(x => new DO_Forms
                      {
                          FormID = x.f.FormId,
                          FormName = x.f.FormName,
                          FormCode = x.f.FormCode
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
        public async Task<List<DO_ApprovalLevels>> GetApprovalLevelsbyCodeType(int codetype,int businesskey,int formId,int approvaltype)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    if (approvaltype == 70001)
                    {
                        var ds = await db.GtEcapcds.Where(x => x.CodeType == codetype && x.ActiveStatus)
                       .GroupJoin(db.GtEcapvds.Where(x => x.BusinessKey == businesskey && x.FormId == formId),
                        lscn => new { ApprovalLevel = lscn.ApplicationCode },
                          r => new { r.ApprovalLevel },
                         (lscn, r) => new { lscn, r })

                        .SelectMany(z => z.r.DefaultIfEmpty(),
                         (h, b) => new DO_ApprovalLevels
                         {
                             ApprovalLevel = h.lscn.ApplicationCode,
                             ApprovalLevelDesc = h.lscn.CodeDesc,
                             ActiveStatus = b != null ? b.ActiveStatus : false,

                         }
                       ).ToListAsync();

                        var distinct = ds
                .GroupBy(x => new { x.ApprovalLevel })
                .Select(g => g.First())
                .ToList();
                        return distinct;
                    }
                    else
                    {
                        var ds = await db.GtEcapcds.Where(x => x.CodeType == codetype && x.ActiveStatus)
                       .GroupJoin(db.GtEcapvvs.Where(x => x.BusinessKey == businesskey && x.FormId == formId),
                        lscn => new { ApprovalLevel = lscn.ApplicationCode },
                          r => new { r.ApprovalLevel },
                         (lscn, r) => new { lscn, r })

                        .SelectMany(z => z.r.DefaultIfEmpty(),
                         (h, b) => new DO_ApprovalLevels
                         {
                             ApprovalLevel = h.lscn.ApplicationCode,
                             ApprovalLevelDesc = h.lscn.CodeDesc,
                             ActiveStatus = b != null ? b.ActiveStatus : false,

                         }
                       ).ToListAsync();
                        var distinct = ds
                   .GroupBy(x => new { x.ApprovalLevel })
                   .Select(g => g.First())
                   .ToList();
                        return distinct;
                    }
                   
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<DO_ReturnParameter> InsertOrUpdateApprovalLevel(DO_ApprovalTypes obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEcapvh _isExist = db.GtEcapvhs.Where(c => c.BusinessKey == obj.BusinessKey && c.FormId == obj.FormId).FirstOrDefault();

                        if (_isExist != null)
                        {
                            _isExist.ApprovalType = obj.ApprovalType;
                            _isExist.ActiveStatus = obj.ActiveStatus;
                            _isExist.ModifiedBy = obj.UserID;
                            _isExist.ModifiedOn = DateTime.Now;
                            _isExist.ModifiedTerminal = obj.TerminalID;
                            db.SaveChanges();
                        }
                        else 
                        {
                            var apprl = new GtEcapvh()
                            {
                                BusinessKey = obj.BusinessKey,
                                FormId = obj.FormId,
                                ApprovalType=obj.ApprovalType,
                                ActiveStatus = obj.ActiveStatus,
                                CreatedBy = obj.UserID,
                                CreatedOn = DateTime.Now,
                                CreatedTerminal = obj.TerminalID
                            };
                            db.GtEcapvhs.Add(apprl);
                            db.SaveChanges();
                        }
                        List<GtEcapvd> apprtypes = db.GtEcapvds.Where(c => c.FormId == obj.FormId && c.BusinessKey == obj.BusinessKey).ToList();
                        if (apprtypes.Count > 0)
                        {
                            foreach (var ap in apprtypes)
                            {
                                db.GtEcapvds.Remove(ap);
                                db.SaveChanges();
                            }

                        }
                        if (obj.lst_ApprovalLevels.Count>0)
                        {
                            foreach (var aprl in obj.lst_ApprovalLevels.Where(x=>x.ActiveStatus))
                            {
                                GtEcapvd aptype = new GtEcapvd
                                {
                                    ApprovalLevel = aprl.ApprovalLevel,
                                    BusinessKey = obj.BusinessKey,
                                    ActiveStatus = aprl.ActiveStatus,
                                    FormId = obj.FormId,
                                    CreatedBy = obj.UserID,
                                    CreatedOn = DateTime.Now,
                                    CreatedTerminal = obj.TerminalID
                                };
                                db.GtEcapvds.Add(aptype);
                                await db.SaveChangesAsync();

                            }

                            dbContext.Commit();
                            return new DO_ReturnParameter() { Status = true, StatusCode = "S0001", Message = string.Format(_localizer[name: "S0001"]) };
                        }

                        else
                        {
                            dbContext.Commit();
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0001", Message = string.Format(_localizer[name: "W0001"]) };
                        }
                    }
                    catch (DbUpdateException ex)
                    {
                        dbContext.Rollback();
                        throw new Exception(CommonMethod.GetValidationMessageFromException(ex));
                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }
        }
        #endregion

        #region Approval Value Based
        public async Task<DO_ReturnParameter> InsertOrUpdateApprovalValueBased(DO_ApprovalTypes obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEcapvh _isExist = db.GtEcapvhs.Where(c => c.BusinessKey == obj.BusinessKey && c.FormId == obj.FormId).FirstOrDefault();

                        if (_isExist != null)
                        {
                            _isExist.ApprovalType = obj.ApprovalType;
                            _isExist.ActiveStatus = obj.ActiveStatus;
                            _isExist.ModifiedBy = obj.UserID;
                            _isExist.ModifiedOn = DateTime.Now;
                            _isExist.ModifiedTerminal = obj.TerminalID;
                            db.SaveChanges();
                        }
                        else
                        {
                            var apprl = new GtEcapvh()
                            {
                                BusinessKey = obj.BusinessKey,
                                FormId = obj.FormId,
                                ApprovalType = obj.ApprovalType,
                                ActiveStatus = obj.ActiveStatus,
                                CreatedBy = obj.UserID,
                                CreatedOn = DateTime.Now,
                                CreatedTerminal = obj.TerminalID
                            };
                            db.GtEcapvhs.Add(apprl);
                            db.SaveChanges();
                        }
                        List<GtEcapvd> apprtypes = db.GtEcapvds.Where(c => c.FormId == obj.FormId && c.BusinessKey == obj.BusinessKey).ToList();
                        if (apprtypes.Count > 0)
                        {
                            foreach (var ap in apprtypes)
                            {
                                db.GtEcapvds.Remove(ap);
                                db.SaveChanges();
                            }

                        }

                        if (obj.lst_ApprovalLevels.Count > 0)
                        {
                            foreach (var aprl in obj.lst_ApprovalLevels.Where(x=>x.ActiveStatus))
                            {
                                GtEcapvd aptype = new GtEcapvd
                                {
                                    ApprovalLevel = aprl.ApprovalLevel,
                                    BusinessKey = obj.BusinessKey,
                                    ActiveStatus = aprl.ActiveStatus,
                                    FormId = obj.FormId,
                                    CreatedBy = obj.UserID,
                                    CreatedOn = DateTime.Now,
                                    CreatedTerminal = obj.TerminalID
                                };
                                db.GtEcapvds.Add(aptype);
                                await db.SaveChangesAsync();

                            }

                        }

                        if(obj.lst_ApprovalValues.Count>0)
                        {

                            {
                                foreach (var val in obj.lst_ApprovalValues)
                                {
                                   


                                    var valExist = db.GtEcapvvs.Where(x => x.BusinessKey == val.BusinessKey && x.FormId == val.FormId
                                    && x.ApprovalLevel == val.ApprovalLevel && x.ValueFrom == val.ValueFrom && x.EffectiveFrom == val.EffectiveFrom).FirstOrDefault();
                                    
                                    if (valExist != null)
                                    {

                                        valExist.ValueTo = val.ValueTo;
                                        valExist.EffectiveTill = val.EffectiveTill;
                                        valExist.ActiveStatus = val.ActiveStatus;
                                        valExist.ModifiedBy = obj.UserID;
                                        valExist.ModifiedOn = System.DateTime.Now;
                                        valExist.ModifiedTerminal = obj.TerminalID;
                                    }
                                    else if(val.ActiveStatus)
                                    {
                                        GtEcapvv apvalue = new GtEcapvv
                                        {
                                            BusinessKey = obj.BusinessKey,
                                            FormId = obj.FormId,
                                            ApprovalLevel = val.ApprovalLevel,
                                            ValueFrom=val.ValueFrom,
                                            EffectiveFrom=val.EffectiveFrom,
                                            ValueTo=val.ValueTo,
                                            EffectiveTill=val.EffectiveTill,
                                            ActiveStatus = val.ActiveStatus,
                                            CreatedBy = obj.UserID,
                                            CreatedOn = DateTime.Now,
                                            CreatedTerminal = obj.TerminalID
                                        };
                                        db.GtEcapvvs.Add(apvalue);
                                    }

                                    await db.SaveChangesAsync();

                                }

                                dbContext.Commit();
                                return new DO_ReturnParameter() { Status = true, StatusCode = "S0001", Message = string.Format(_localizer[name: "S0001"]) };
                            }
                        }
                        else
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0001", Message = string.Format(_localizer[name: "W0001"]) };
                        }
                    }
                    catch (DbUpdateException ex)
                    {
                        dbContext.Rollback();
                        throw new Exception(CommonMethod.GetValidationMessageFromException(ex));
                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }
        }

        public async Task<List<DO_ApprovalValues>> GetApprovalValuesbyFormID(int businesskey, int formId)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = await db.GtEcapvvs.Where(x => x.BusinessKey == businesskey && x.FormId == formId)
                          .Select(z => new DO_ApprovalValues
                          {
                              ApprovalLevel = z.ApprovalLevel,
                              ValueFrom = z.ValueFrom,
                              ValueTo = z.ValueTo,
                              EffectiveFrom = z.EffectiveFrom,
                              EffectiveTill = z.EffectiveTill,
                              ActiveStatus = z.ActiveStatus,
                          }
                        ).ToListAsync();


                    var distinct = ds
                    .GroupBy(x => new { x.ValueFrom, x.ValueTo,x.EffectiveFrom,x.EffectiveTill })
                    .Select(g => g.First())
                    .ToList();

                    return distinct;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
