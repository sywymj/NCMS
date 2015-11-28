using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NCMS_Local.DTO
{
    [Serializable]
    public class PatientInfo
    {
        public string Name { get; set; }
        public int? Age { get; set; }
        public string AgeUnit { get; set; }
        public DateTime? BirthDay { get; set; }
        public string Marray { get; set; }
        public string Sex { get; set; }
        public string PSN { get; set; }

        public int? HisZyh { get; set; }
        public int? HisRyh
        {
            get { return HisZyh; }
            set { HisZyh = value; }
        }
        public short? HisNationCode { get; set; }
        public string HisNation { get; set; }

        public int? HisRyksCode { get; set; }
        public string HisRyksDesc{get;set;}
        public int? HisZybrlx { get; set; }
        public DateTime? Ryrq { get; set; }
        public int? HisCzyCode { get; set; }

        public string HisMzzdCode { get; set; }
        public string HisMzzdDesc { get; set; }
        public string HisRyzdCode{get;set;}
        public string HisRyzdDesc{get;set;}
        public int? HisDoctorCode{get;set;}
        public string HisDoctorName{get;set;}

    }
}
