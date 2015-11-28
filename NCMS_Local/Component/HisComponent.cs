using System;
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
                var zybr=db.ZYBR.Where(b => b.ZYH == pInfo.HisZyh && b.RY.First().CYRQ == null).FirstOrDefault();
                if (zybr==null)
                {
                    pInfo.HisZyh= db.ExecuteQuery<int>("SELECT MAXVAL=ISNULL(MAX(RYH),0)+1 FROM RY").First();
                    zybr = new ZYBR();
                    zybr.ZYH = pInfo.HisZyh.Value;
                    zybr.MZDM = pInfo.HisNationCode.Value;
                    zybr.BRXM = pInfo.Name;
                    zybr.XB = pInfo.Sex;
                    zybr.CSRQ = pInfo.BirthDay;
                    zybr.HYZK = pInfo.Marray;
                    zybr.GJ = "中国";
                    zybr.SFZH = pInfo.PSN;
                    zybr.NL = pInfo.Age;
                    zybr.JSDW = pInfo.AgeUnit;

                    db.ZYBR.InsertOnSubmit(zybr);

                } 
                else
                {
                }

                return pInfo.HisZyh??-1;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
            	return -1;
            }
        }
    }
}
