using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NCMS_Local.DTO
{
    public class ParamBalance
    {
        public int zyh { get; set; }
        public DateTime outDate { get; set; }
        public CIll cyIll { get; set; }
        public bool isForced { get; set; }
        
    }
}
