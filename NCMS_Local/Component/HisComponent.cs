using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NCMS_Local.LTSQL;
using NCMS_Local.DTO;
using NCMS_Local.NHFUN;
using System.Data;
using System.Data.SqlClient;

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
            nhPerson.InDate = (pInfo.Ryrq ?? DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss");
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

        
        public int JzdToNhFeeListByZyh(int zyh)
        {
            DCCbhisDataContext hisDb = new DCCbhisDataContext(GSettings.HisConnStr);
            //DCNhDataContext nhDb = new DCNhDataContext(GSettings.NhConnStr);
            int error = 0;
            try
            {
                var ypjzds = from ypjzd in hisDb.JZD where ypjzd.ZYH == zyh && ypjzd.ZF == 0  && ypjzd.HJDH != null && ypjzd.SB_UPLOAD==0 select ypjzd;
#region 处理药品
                foreach (var jzd in ypjzds)
                {
                    //开始处理一张药品记账单
                    try
                    {
                        var nhFeeItems = from _hjdmx in hisDb.HJDMX
                                         join _hjd in hisDb.HJD on _hjdmx.HJDH equals _hjd.HJDH
                                         where _hjd.JZDH == jzd.JZDH
                                         select new
                                         {
                                             Cfh = _hjd.JZDH,
                                             xh = _hjdmx.XH,
                                             Zyh = jzd.ZYH,
                                             HosCode = _hjdmx.YPGGDM,
                                             UseDate = _hjd.HJRQ,
                                             Price = _hjdmx.JE / _hjdmx.SL,
                                             Num = _hjdmx.SL,
                                             Fee = _hjdmx.JE,
                                             OfficeName = _hjd.KDKS,
                                             Doctor = _hjd.ZG_YS.ZGXM
                                         };
                        foreach (var _item in nhFeeItems)
                        {
                            hisDb.WyNhFeeList.InsertOnSubmit(new WyNhFeeList()
                            {
                                Cfh = string.Format("{0}{1:D3}", _item.Cfh, _item.xh
                                    ),
                                Zyh = _item.Zyh,
                                HosCode = string.Format("YP{0}", _item.HosCode),
                                UseDate = _item.UseDate.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                                Price = _item.Price,
                                Num = _item.Num,
                                Fee = _item.Fee,
                                OfficeName = Convert.ToString(_item.OfficeName),
                                Doctor = _item.Doctor
                            });
                        }
                        jzd.SB_UPLOAD = 1;
                        hisDb.SubmitChanges();
                    }
                    catch (System.Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        error++;
                    }
                }
#endregion

                var zljzds = from ypjzd in hisDb.JZD where ypjzd.ZYH == zyh && ypjzd.ZF == 0 && ypjzd.HJDH == null && ypjzd.SB_UPLOAD == 0 select ypjzd;
#region 处理诊疗
                foreach (var jzd in zljzds)
                {
                    //开始处理一张诊疗单据
                    try
                    {
                        foreach (var item in jzd.JZDMX)
                        {
                            hisDb.WyNhFeeList.InsertOnSubmit(new WyNhFeeList()
                            {
                                Cfh = string.Format("{0}{1:D3}", jzd.JZDH, item.XH),
                                Zyh=jzd.ZYH,
                                HosCode=string.Format("ZL{0}",item.SFXMDM),
                                UseDate=jzd.RQ.ToString("yyyy-MM-dd HH:mm:ss"),
                                Price=item.JE/item.CS??1,
                                Num=item.CS??1,
                                Fee=item.JE,
                                OfficeName=Convert.ToString(jzd.ZXKS),
                                Doctor=jzd.ZG_jzd_ys.ZGXM                                
                            });
                        }
                        jzd.SB_UPLOAD = 1;
                        hisDb.SubmitChanges();
                    }
                    catch (System.Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        error++;
                    }
                }
#endregion
            }
            catch (System.Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return error;
        }

        public WyNhRegister GetNhPersonInfoByZyh(int zyh)
        {
            try
            {
                DCCbhisDataContext hisDb = new DCCbhisDataContext(GSettings.HisConnStr);
                DCNhDataContext nhDb = new DCNhDataContext(GSettings.NhConnStr);
                var feeItems = from _f in hisDb.WyNhFeeList where _f.Zyh == zyh && _f.FeeNo == null select _f;
                var _NhPersonInfo = (from _f in hisDb.WyNhRegister where _f.Zyh == zyh && _f.IsFail == (byte)0 select _f).FirstOrDefault();
                return _NhPersonInfo;
            }
            catch (System.Exception ex)
            {
                return null;
            }
        }
        public string ClearAllUploadedFeeByZyh(int zyh)
        {
            string hr = string.Empty;
            StringBuilder sb = null;
            int iHr = 0;


            System.Data.SqlClient.SqlConnection conn=new System.Data.SqlClient.SqlConnection(GSettings.HisConnStr);
            DCCbhisDataContext hisDb = new DCCbhisDataContext(conn);
            System.Data.SqlClient.SqlTransaction trans=null;
            try
            {
                var _NhPersonInfo = GetNhPersonInfoByZyh(zyh);
                if (_NhPersonInfo == null)
                {
                    throw new Exception("获取农合患者信息错误！");
                }
                sb = new StringBuilder(256);
                iHr = NhLocalWrap.DeleteFeeList(
                    string.Format("{0}$${1}", _NhPersonInfo.OrganCode, _NhPersonInfo.AccountYear),
                                _NhPersonInfo.CoopMedCode,
                                _NhPersonInfo.AiIDNo,
                                int.Parse(_NhPersonInfo.DiagNo),
                                sb
                    );
                if (iHr<0)
                {
                    throw new Exception(sb.ToString());
                }
                conn.Open();
                trans = conn.BeginTransaction();
                hisDb.Transaction = trans;
                hisDb.ExecuteCommand("delete from wynhfeelist where zyh={0}", zyh);
                hisDb.ExecuteCommand("update jzd set sb_upload=0 where zyh={0}", zyh);

                trans.Commit();
            }
            catch (System.Exception ex)
            {
                if (trans!=null)
                {
                    trans.Rollback();
                }
                hr = ex.Message;
            }
            finally{
                conn.Close();
            }
            return hr;
        }


        public IEnumerable<string> ProcessFeeListByZyh(int zyh,bool Direct)
        {
            List<string> lsNoNhCodes = new List<string>();
            DCCbhisDataContext hisDb = new DCCbhisDataContext(GSettings.HisConnStr);
            DCNhDataContext nhDb = new DCNhDataContext(GSettings.NhConnStr);

            var _NhPersonInfo = GetNhPersonInfoByZyh(zyh);
            if (_NhPersonInfo==null)
            {
                lsNoNhCodes.Add(" 未找到患者有效的农合入院登记信息");
                return lsNoNhCodes;
            }
            StringBuilder sb = null;
            int hr = -1;

            var feeItems = from _f in hisDb.WyNhFeeList where _f.Zyh == zyh && _f.FeeNo == null select _f;

            foreach (var feeItem in feeItems)
            {
                try
                {
                    var _nhcode = (from _f in nhDb.P_HiHosItem where _f.HosCode == feeItem.HosCode && _f.OrganId == _NhPersonInfo.OrganCode && _f.ztyear == _NhPersonInfo.AccountYear select _f).FirstOrDefault();

                    //var _nhcode = (from _f in nhDb.P_HiHosItem where _f.HosCode == feeItem.HosCode && _f.OrganId =="420302" && _f.ztyear == _NhPersonInfo.AccountYear select _f).FirstOrDefault();
                    if (_nhcode==null)
                    {
                        lsNoNhCodes.Add(string.Format(@"{0}农合项目编码为空",  feeItem.HosCode));
                    }
                    if (Direct || _nhcode!=null)
                    {
                        sb = new StringBuilder(256);
                        hr = NhLocalWrap.SaveFreeList(
                            string.Format("{0}$${1}", _NhPersonInfo.OrganCode, _NhPersonInfo.AccountYear),
                            _NhPersonInfo.CoopMedCode,
                            _NhPersonInfo.AiIDNo,
                            int.Parse(_NhPersonInfo.DiagNo),
                            null,
                            feeItem.HosCode,
                            //feeItem.UseDate,
                            DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                            (double)feeItem.Price, (double)feeItem.Num, (double)feeItem.Fee,
                            null,
                            feeItem.OfficeName,
                            feeItem.Doctor,
                            "1",
                            sb
                            );
                        if (hr<0)
                        {
                            throw new Exception(string.Format(@"{0}单据上传错误:{1}", feeItem.Cfh, sb.ToString()));
                        }
                        if (_nhcode!=null)
                        {
                            feeItem.NhCode = _nhcode.InCode;
                        }
                        feeItem.FeeNo = sb.ToString();
                        hisDb.SubmitChanges();
                    }

                    
                }
                catch (System.Exception ex)
                {
                    lsNoNhCodes.Add(ex.Message);
                }
            }

            sb = new StringBuilder(256);
            hr = NhLocalWrap.PreClearing(
                            string.Format("{0}$${1}", _NhPersonInfo.OrganCode, _NhPersonInfo.AccountYear),
                            _NhPersonInfo.CoopMedCode,
                            _NhPersonInfo.AiIDNo,
                            int.Parse(_NhPersonInfo.DiagNo),
                            0, 2,
                            DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                            DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                            "1",
                            sb
                            );

            return lsNoNhCodes;
        }

        public string HisBalance(ParamBalance pb)
        {
            int zyh = pb.zyh;
            string hr = string.Empty;

            SqlConnection hisConn = new SqlConnection(GSettings.HisConnStr);
            SqlTransaction hisTrans = null;
            DCCbhisDataContext hisDb = new DCCbhisDataContext(hisConn);

            try
            {
                hisConn.Open();

                var brInfo = (from _r in hisDb.RY where _r.ZYH == zyh && _r.ZT == 0 && _r.ZF == 0 select _r).FirstOrDefault();
                if (brInfo==null)
                {
                    throw new Exception("未找到患者有效的在院信息或者该患者已办理出院");
                }


                //记床位费;
                hisDb.ExecuteCommand("exec 住院收费_记单个病人床位费 {0}", zyh);
                //查询已记账未发药的单据
                var noPutHjds = hisDb.ExecuteQuery<string>(@"select a.hjdh from jzd a join hjd b on a.hjdh=b.hjdh join bm c on a.kdks=c.bmdm	join bm d on a.zxks=d.bmdm	join zg e on a.czy=e.zgdm	left join zg f on a.ys=f.zgdm where a.zyh={0} and a.hjdh is not null and b.fyrq is null and b.zf<>1", zyh).ToArray();
                if (noPutHjds!=null&&noPutHjds.Length>0)
                {
                    throw new Exception(string.Format(@"划价单：{0} 已记账未发药！", string.Join(",", noPutHjds)));
                }

                //查询划价单已作废，记账单未作废的单据
                var noDelInvoidJzds = hisDb.ExecuteQuery<string>(@"select jzdh from jzd where jzdh in (select jzdh from hjd where zf=1) and zyh={0} and zf=0",zyh).ToArray();
                if (noDelInvoidJzds!=null &&noDelInvoidJzds.Length>0)
                {
                    throw new Exception(string.Format(@"划价单：{0} 已记账未发药！", string.Join(",", noDelInvoidJzds)));
                }
                var operatorTicketSerial = (from _t in hisDb.DQDJH where _t.CZY == GSettings.OperatorID && _t.DJLXDM==4 select _t).FirstOrDefault();
                if (operatorTicketSerial==null)
                {
                    throw new Exception("当前操作员未分配单据号!");
                }
                int curTickSerail = hisDb.ExecuteQuery<int>("SELECT SJH=ISNULL(MAX(CYFPH),1) FROM CYFP WHERE CYFPH>0 AND CZY={0}", GSettings.OperatorID).First();
                if (curTickSerail==1)
                {
                    curTickSerail = operatorTicketSerial.KSDJH.Value;
                } 
                else
                {
                    curTickSerail += 1;
                }
                if (curTickSerail>operatorTicketSerial.JSDJH.Value)
                {
                    throw new Exception("当前操作员出院发票已用完，请联系分配人员重新分配住院发票！");
                }
                int cyxh = hisDb.ExecuteQuery<int>("select isnull(max(cyxh),0)+1 from cy").FirstOrDefault();
                var sumFee = hisDb.ExecuteQuery<OutValue_sp_GetInHosSumFee_List>("exec sp_GetInHosSumFee_List {0}", zyh).FirstOrDefault();
                var zjBqye = hisDb.ExecuteQuery<decimal>(@"select top 1 bqye from cyfp where zyh={0} and cyxh is null order by rq desc, ztjz_zzrq desc, cyfph desc", zyh).FirstOrDefault();
                sumFee.yjk += zjBqye;

                DateTime serverTime = hisDb.ExecuteQuery<DateTime>("select getdate()").FirstOrDefault();


                //开始事务处理
                hisTrans = hisConn.BeginTransaction();
                hisDb.Transaction = hisTrans;
                //①，insert to cy
                CY newCy = new CY();
                newCy.CYXH = cyxh;
                newCy.ZYH = zyh;
                newCy.CYFPH = curTickSerail;
                newCy.ksdm = brInfo.KSDM.Value;
                newCy.CYRQ = pb.outDate;
                newCy.HJJE = sumFee.zje;
                newCy.HJJE_YS = sumFee.yszje;
                newCy.YJJE = sumFee.yjk;
                newCy.BJJE = sumFee.yszje - sumFee.yjk;
                newCy.QFJE = 0; newCy.QF = 0; newCy.BXJE = 0;
                newCy.QFLBMC = null;
                newCy.czy = GSettings.OperatorID;
                newCy.fkfs = "现金";
                newCy.jsdw = "";
                newCy.ZF = 0;
                newCy.jbsj = serverTime;
                hisDb.CY.InsertOnSubmit(newCy);
                hisDb.SubmitChanges();
                //②，insert to cyfp
                hisDb.ExecuteCommand(@"INSERT INTO CYFP(CYFPH, CYXH, RQ, JSDW, HJJE, HJJE_YS, YJJE, BJJE, CZY, BXJE, ZYH, SQYE, ZF) SELECT CYFPH,CYXH, CYRQ,'', HJJE, HJJE_YS, YJJE, BJJE, CZY, BXJE, ZYH, {1}, 0 FROM CY WHERE CYXH = {0}", cyxh,zjBqye);
                //③，insert to cyfpmx
                var cyfpmxs = hisDb.ExecuteQuery<OutValue_sp_GetInHosSumFee>("exec sp_GetInHosSumFee @Zyh={0}, @LSH='%'",zyh);
                short _xh = 1;
                foreach (var cyfpmx in cyfpmxs)
                {
                    hisDb.CYFPMX.InsertOnSubmit(new CYFPMX() {CYFPH=curTickSerail,XH=_xh++,YJKMDM=cyfpmx.yjkmdm,JE=cyfpmx.zje,YSJE=cyfpmx.ysje });
                }
                //④更新相关表，Ry，zybr_child,bc,bcsyjl
                brInfo.ZT = 1;
                brInfo.JBSJ = serverTime;
                brInfo.CYRQ = pb.outDate;
                brInfo.CYBJJE = -1 * brInfo.YE;
                brInfo.YE = 0;
                hisDb.SubmitChanges();

                hisDb.ExecuteCommand(@"update zybr_child set cyrq={0} where zyh={1}", pb.outDate, zyh);
                hisDb.ExecuteCommand(@"UPDATE BC SET ZYH=NULL,FCRQ=NULL, FYJSRQ=NULL WHERE ZYH={0}",zyh);
                hisDb.ExecuteCommand(@"UPDATE BCSYJL SET JSRQ=b.CYRQ, BS=2 FROM BCSYJL a JOIN RY b ON a.ZYH = b.ZYH WHERE b.ZYH={0} AND a.JSRQ IS NULL", zyh);

                
                hisTrans.Commit();
            }
            catch (System.Exception ex)
            {
                hr = ex.Message;
                if (hisTrans!=null)
                {
                    hisTrans.Rollback();
                }
            }
            finally
            {
                hisConn.Close();
            }
            return hr;
        }

        public string HisBalanceDel(int zyh)
        {
            string hrString = string.Empty;

            SqlConnection hisConn = new SqlConnection(GSettings.HisConnStr);
            SqlTransaction hisTrans = null;
            DCCbhisDataContext hisDb = new DCCbhisDataContext(hisConn);

            try
            {
                hisConn.Open();
                DateTime serverTime = hisDb.ExecuteQuery<DateTime>(@"select getdate()").First();

                var cyInfo = (from _cy in hisDb.CY where _cy.ZYH == zyh && _cy.ZF == 0 orderby _cy.CYXH descending select _cy).FirstOrDefault();                
                var cyfpInfo=(from _cyfp in hisDb.CYFP where _cyfp.CYXH==cyInfo.CYXH && _cyfp.lsh==null select _cyfp).FirstOrDefault();
                if (cyInfo == null || cyfpInfo==null)
                {
                    throw new Exception("未找到有效的出院结算信息");
                }

                hisTrans = hisConn.BeginTransaction();
                hisDb.Transaction = hisTrans;
                //开始处理出院作废事务
                hisDb.ExecuteCommand(@"insert into cy(cyxh,zyh,cyfph,ksdm,cyrq,hjje,hjje_ys,yjje,bjje,qfje,qf,zfczy,bxje, qflbmc,czy,zfrq, fkfs,jsdw, JBSJ, ChildHJJE) select -cyxh,zyh,cyfph=(case when cyfph is null then null else -cyfph end),ksdm,{0},-hjje,-hjje_ys,-yjje,-bjje,-qfje,qf,{1},-bxje,qflbmc,{1},{0},fkfs,jsdw, GetDate(), -ChildHJJE from cy where cyxh={2}", serverTime, GSettings.OperatorID, cyInfo.CYXH);
                hisDb.ExecuteCommand(@"insert into cyfp(cyfph,cyxh,rq,jsdw,hjje,hjje_ys,yjje,bjje,czy,bxje,zyh,SQYE, LSH, MotherCYFPH, ChildHJJE) select -cyfph,-cyxh,{0},jsdw,-hjje,-hjje_ys,-yjje,-bjje,{1},-bxje,zyh,-SQYE, LSH, -MotherCYFPH, -ChildHJJE from cyfp where cyxh={2} and zf=0",serverTime,GSettings.OperatorID,cyInfo.CYXH);
                hisDb.ExecuteCommand(@"insert into cyfpmx(cyfph,xh,yjkmdm,je, ysje, MotherCYFPH) select -cyfph,xh,yjkmdm,-je, -ysje, -MotherCYFPH from cyfpmx where cyfph in(select cyfph from cyfp where cyxh={0} and zf=0)", cyInfo.CYXH);
                hisDb.ExecuteCommand(@"update cyfp set zf=1,zfczy={0} where abs(cyfph) in (select cyfph from cyfp where cyxh={1} and zf=0)", GSettings.OperatorID, cyInfo.CYXH);
                hisDb.ExecuteCommand(@"update cy set zf=1,zfczy={0} where abs(cyxh)={1}", GSettings.OperatorID, cyInfo.CYXH);
                hisDb.ExecuteCommand(@"update ry set cyrq=null,jbsj = null,zt=0,ye=-cybjje,cybjje=0 where zyh={0}", cyInfo.ZYH);
                
                hisTrans.Commit();
            }
            catch (System.Exception ex)
            {
                hrString = ex.Message;
                if (hisTrans!=null)
                {
                    hisTrans.Rollback();
                }
            }
            finally
            {
                hisConn.Close();
            }
            return hrString;
        }
    }


    class OutValue_sp_GetInHosSumFee_List
    {
        public decimal zje { get; set; }
        public decimal yszje { get; set; }
        public decimal yjk { get; set; }
        public decimal xsejzje { get; set; }
        public decimal xsecwf { get; set; }
    }
    class OutValue_sp_GetInHosSumFee
    {
        public short yjkmdm { get; set; }
        public string yjkmmc { get; set; }
        public decimal zje { get; set; }
        public decimal ysje { get; set; }
    }
}
