using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NCMS_Local.LTSQL;
using NCMS_Local.DTO;

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



        public int InpatientRegister(DTO.PatientInfo pInfo)
        {
            LTSQL.DCCbhisDataContext db = new LTSQL.DCCbhisDataContext(_hisConn);
            db.Log = Console.Out;

            try
            {
                var zybr=db.ZYBR.Where(b => b.ZYH == pInfo.HisZyh && b.RY.First().CYRQ == null && string.Compare(b.BRXM.Trim(),pInfo.Name.Trim(),true)==0).FirstOrDefault();
                RY ry = null;
                BASY basy = null;
                if (zybr==null)
                {
                    pInfo.HisZyh= db.ExecuteQuery<int>("SELECT MAXVAL=ISNULL(MAX(RYH),0)+1 FROM RY").First();
                    zybr = new ZYBR();
                    ry = new RY();
                    basy = new BASY();



                    PInfoToEntitys(pInfo, zybr, ry, basy);

                    db.ZYBR.InsertOnSubmit(zybr);
                    db.RY.InsertOnSubmit(ry);
                    db.BASY.InsertOnSubmit(basy);
                } 
                else
                {
                    ry = zybr.RY.First();
                    basy = ry.BASY;
                    PInfoToEntitys(pInfo, zybr, ry, basy);
                }
                db.SubmitChanges();
                return pInfo.HisZyh??-1;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
            	return -1;
            }
        }

        private static void PInfoToEntitys(DTO.PatientInfo pInfo, ZYBR zybr, RY ry, BASY basy)
        {
            zybr.ZYH = pInfo.HisZyh.Value;
            zybr.MZDM = (short)pInfo.HisNationCode;
            zybr.BRXM = pInfo.Name;
            zybr.XB = pInfo.Sex;
            zybr.CSRQ = pInfo.BirthDay;
            zybr.HYZK = pInfo.Marray;
            zybr.GJ = "中国";
            zybr.SFZH = pInfo.PSN;
            zybr.NL = pInfo.Age;
            zybr.JSDW = pInfo.AgeUnit;


            ry.ZYH = pInfo.HisZyh.Value;
            ry.RYH = pInfo.HisRyh.Value;
            ry.RYKS = (short)pInfo.oZyDoctor.bm.bmdm;
            ry.ZYBRLX = (Byte)pInfo.HisZybrlx;
            ry.RYRQ = pInfo.Ryrq.Value;
            
            //ry.JBSJ = DateTime.Now;

            ry.RYQKDM = 1;
            ry.YS = (short)pInfo.oZyDoctor.zgdm;
            ry.CZY = (short)GOperator.ID;

            ry.RYCH = 0;
            ry.KSDM = (short)pInfo.oZyDoctor.bm.bmdm;


            basy.RYH = pInfo.HisZyh.Value;
            basy.RYZD_ICD = pInfo.oRyIll.IllCode;
            basy.RYZD = pInfo.oRyIll.IllDesc;
            basy.MZZD_YS = (short)pInfo.oMzDoctor.zgdm;
        }
    }
}
