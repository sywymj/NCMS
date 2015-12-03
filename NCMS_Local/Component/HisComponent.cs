using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NCMS_Local.LTSQL;
using NCMS_Local.DTO;
using NCMS_Local.NHFUN;

namespace NCMS_Local
{
    [Serializable]
    public class HisComponent
    {
        private string _hisConn = string.Empty;
        public HisComponent(){
            this._hisConn=GSettings.HisConnStr;
        }
        public IEnumerable<CDoctor> GetDoctors()
        {
            LTSQL.DCCbhisDataContext db = new LTSQL.DCCbhisDataContext(_hisConn);
            try
            {
                return (from b in db.ZG select new CDoctor {
                zgdm= b.ZGDM,
                zgmc=b.ZGXM,
                pym = b.PYM,
                zglb = (int)b.ZGLBDM,
                bm = new CDepart() { bmdm=(int)b.BMDM,bmmc=b.BM.BMMC}
                }).ToArray();
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }
        public int MakeZyh()
        {
            try
            {
                using (LTSQL.DCCbhisDataContext db = new LTSQL.DCCbhisDataContext(_hisConn))
                {
                    return db.ExecuteQuery<int>("SELECT MAXVAL=ISNULL(MAX(RYH),0)+1 FROM RY").First();
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
                return -1;
            }
            
           

        }

        public int NewPatientRegister(DTO.PatientInfo pInfo)
        {
            try
            {
                var zybr = new ZYBR();
                var ry = new RY();
                var basy = new BASY();

                using (DCCbhisDataContext db = new DCCbhisDataContext(GSettings.HisConnStr))
                {
                    zybr.ZYH = MakeZyh();
                    PInfoToEntitys(pInfo, zybr, ry, basy);

                    db.ZYBR.InsertOnSubmit(zybr);
                    db.RY.InsertOnSubmit(ry);
                    db.BASY.InsertOnSubmit(basy);

                    db.SubmitChanges();
                }
                return zybr.ZYH;
            }
            catch (System.Exception ex)
            {
                throw new ArgumentException("His入院登记错误:"+ex.Message);
            }
            
        }
        public Guid NewNhRegister(DTO.PatientInfo pInfo)
        {
            WyNhRegister newNhReg = null;
            int hr = -1;
            StringBuilder sb = null;
            try
            {
                if (pInfo.HisZybrlx == EnumRyLb.农村合作医疗病人 && pInfo.NhInfo is NhPersonInfoBase)
                {
                    //如果是农合病人，则判断是本地农合还是异地农合，这个是通过pinfo的类型来判断
                    if (pInfo.NhInfo is HrGetHzPersonInfo)
                    {
                        //本地农合患者：
                        newNhReg = PinfoToWyNhRegister(pInfo);
                        using (DCCbhisDataContext db = new DCCbhisDataContext(GSettings.HisConnStr))
                        {
                            db.WyNhRegister.InsertOnSubmit(newNhReg);
                            db.SubmitChanges();
                            //开始调用接口上传注册信息
                            sb = new StringBuilder(1024);
                            hr = NhLocalWrap.SaveInHosInfo(GSettings.ParamLocalOrganID, newNhReg.CoopMedCode, newNhReg.ExpressionID, newNhReg.PatientName, newNhReg.AiIDNo, newNhReg.IllCode, newNhReg.IllName, newNhReg.InDate, newNhReg.Adke, newNhReg.AdLimitDef, newNhReg.DoctorName, newNhReg.PatientID, "0", newNhReg.DiagNo, newNhReg.ExpenseKind, newNhReg.LimitIllCode, sb);
                            if (hr<0)
                            {
                                //接口调用失败,删除已保存在本地的农合登记信息，并抛出异常
                                db.WyNhRegister.DeleteOnSubmit(newNhReg);
                                db.SubmitChanges();
                                throw new Exception(sb.ToString());
                            } 
                            else
                            {
                                newNhReg.DiagNo = sb.ToString();
                                db.SubmitChanges();
                            }

                        }            
                        //本地农合插入结束
                    }
                    else
                    {
                        //异地农合插入开始
                        newNhReg = PinfoToWyNhRegister(pInfo);
                        using (DCCbhisDataContext db = new DCCbhisDataContext(GSettings.HisConnStr))
                        {
                            db.WyNhRegister.InsertOnSubmit(newNhReg);
                            db.SubmitChanges();
                            //开始调用接口上传注册信息
                            sb = new StringBuilder(1024);
                            hr = NhLocalWrap.zzSaveInHosInfo(GSettings.ParamRemoteOrganID, newNhReg.AreaCode, newNhReg.CoopMedCode, newNhReg.ExpressionID, newNhReg.AiIDNo, newNhReg.TurnID.Value, newNhReg.IllCode, newNhReg.IllName, newNhReg.InDate, newNhReg.Adke, newNhReg.AdLimitDef, newNhReg.DoctorName, newNhReg.PatientID, newNhReg.ExpenseKind, newNhReg.LimitIllCode, sb);
                            if (hr < 0)
                            {
                                //接口调用失败,删除已保存在本地的农合登记信息，并抛出异常
                                db.WyNhRegister.DeleteOnSubmit(newNhReg);
                                db.SubmitChanges();
                                throw new Exception(sb.ToString());
                            }
                            else
                            {
                                newNhReg.DiagNo = sb.ToString();
                                db.SubmitChanges();
                            }
                        }
                        //异地农合插入结束
                    }
                    return newNhReg.NhRegID;
                }
                else
                {
                    throw new ArgumentException("不是农合患者或者患者类型选择错误");
                }
            }
            catch (System.Exception ex)
            {
                throw new ArgumentException("农合入院登记错误：" + ex.Message);
            }
        }

        private WyNhRegister PinfoToWyNhRegister(PatientInfo pInfo)
        {
            WyNhRegister nhPerson = new WyNhRegister();
            nhPerson.FunHrStr = pInfo.NhInfo.FunHrStr;
            nhPerson.NhRegID = Guid.NewGuid();
            nhPerson.OrganCode = GSettings.OrganIDRemote;
            nhPerson.AccountYear = GSettings.AccountYear;
            nhPerson.CoopMedCode = pInfo.NhInfo.coopMedCode;
            nhPerson.ExpressionID = ((int)pInfo.NhZybxgs).ToString();
            nhPerson.PatientName = pInfo.NhInfo.name;
            nhPerson.AiIDNo = pInfo.NhInfo.aiIDNo;
            nhPerson.IllCode = pInfo.oRyIll.IllCode;
            nhPerson.IllName = pInfo.oRyIll.IllDesc;
            nhPerson.InDate = (pInfo.Ryrq ?? DateTime.Now).ToString("yyyy-MM-dd hh:mm:ss");
            nhPerson.AdLimitDef = GSettings.AdLimitDef;
            nhPerson.DoctorName = string.Format("{0}", pInfo.oZyDoctor.zgmc);
            nhPerson.PatientID = string.Format("{0}@@{1}@@{0}@@{2}",pInfo.HisZyh,pInfo.oZyDoctor.bm.bmdm,GSettings.OperatorName);
            nhPerson.ExpenseKind = ((int)pInfo.NhExpenseKind).ToString();
            nhPerson.IsFail = 0;
            nhPerson.Zyh = pInfo.HisZyh.Value;
            nhPerson.AreaCode = pInfo.NhInfo.areaCode;
            if (pInfo.NhInfo is HrGetZzinfo_zz)
            {
                nhPerson.TurnID = ((HrGetZzinfo_zz)pInfo.NhInfo).transfNo;
            }
         return nhPerson;
        }

        private static void PInfoToEntitys(DTO.PatientInfo pInfo, ZYBR zybr, RY ry, BASY basy)
        {
            zybr.ZYH = pInfo.HisZyh.Value;
            zybr.MZDM = (short)pInfo.HisNationCode;
            zybr.BAH = pInfo.HisZyh.Value;
            zybr.BRXM = pInfo.Name;
            zybr.XB = pInfo.Sex;
            zybr.CSRQ = pInfo.BirthDay;
            zybr.HYZK = pInfo.Marray;
            zybr.GJ = "中国";
            zybr.SFZH = pInfo.PSN;
            zybr.mzks = (short)pInfo.oMzDoctor.bm.bmdm;
            zybr.NL = pInfo.Age;
            zybr.JSDW = pInfo.AgeUnit;


            ry.ZYH = pInfo.HisZyh.Value;
            ry.RYH = pInfo.HisRyh.Value;
            ry.RYKS = (short)pInfo.oZyDoctor.bm.bmdm;
            ry.ZYBRLX = (Byte)pInfo.HisZybrlx;
            ry.RYRQ = pInfo.Ryrq.Value;
            ry.RYQKDM = 1;
            ry.YS = (short)pInfo.oZyDoctor.zgdm;
            ry.CZY = GSettings.OperatorID;
            ry.RYCH = 1;
            ry.KSDM = (short)pInfo.oZyDoctor.bm.bmdm;
            ry.HZ_HZZH = pInfo.NhInfo != null ? pInfo.NhInfo.coopMedCode : null;

            basy.RYH = pInfo.HisZyh.Value;
            basy.RYZD_ICD = pInfo.oRyIll.IllCode;
            basy.RYZD = pInfo.oRyIll.IllDesc;
            basy.MZZD_YS = (short)pInfo.oMzDoctor.zgdm;
        }
    }
}
