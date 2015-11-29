﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NCMS_Local.LTSQL;

namespace NCMS_Local
{
    [Serializable]
    public class HisComponent
    {
        private string _hisConn = string.Empty;
        public HisComponent(string hisConn){
            this._hisConn=hisConn;
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
            zybr.MZDM = pInfo.HisNationCode ?? 1;
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
            ry.RYKS = pInfo.HisRyksCode.Value;
            ry.ZYBRLX = (Byte)pInfo.HisZybrlx.Value;
            ry.RYRQ = pInfo.Ryrq.Value;
            //ry.JBSJ = DateTime.Now;

            ry.RYQKDM = 1;
            ry.YS = pInfo.HisDoctorCode;
            ry.CZY = (short)GOperator.ID;

            ry.RYCH = 0;
            ry.KSDM = pInfo.HisRyksCode;


            basy.RYH = pInfo.HisZyh.Value;
            basy.RYZD_ICD = pInfo.HisRyzdCode;
            basy.RYZD = pInfo.HisRyzdDesc;
            basy.MZZD_YS = pInfo.HisMzDoctorCode;
        }
    }
}
