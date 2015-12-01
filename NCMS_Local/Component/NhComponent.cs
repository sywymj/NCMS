using NCMS_Local.DTO;
using NCMS_Local.LTSQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NCMS_Local.Component
{
    public class NhComponent
    {
        private string _hisConn = string.Empty;
        public NhComponent(string hisConn)
        {
            this._hisConn=hisConn;
        }

        public CIll GetIllsByIllCode(string pym)
        {
            DCNhDataContext db = new DCNhDataContext(_hisConn);
            try
            {
                return (from ii in db.p_Illness
                        where ii.OrganID == "420302" && ii.IllCode==pym
                        select new CIll
                        {
                            IllCode = ii.IllCode,
                            IllDesc = ii.IllName,
                            Spell = ii.Spell
                        }
                            ).FirstOrDefault();
            }
            catch (System.Exception ex)
            {
                return new CIll();
            }
        }
        public IEnumerable<CIll> GetIllsByPym(string pym)
        {
            DCNhDataContext db=new DCNhDataContext(_hisConn);
            try
            {
                return (from ii in db.p_Illness
                        where ii.OrganID == "420302" &&(ii.Spell.Contains(pym)|| ii.IllName.Contains(pym)) 
                        select new CIll
                        {
                            IllCode=ii.IllCode,
                            IllDesc=ii.IllName,
                            Spell=ii.Spell
                        }
                            ).ToArray();
            }
            catch (System.Exception ex)
            {
                return null;
            }
        }
    }
}
