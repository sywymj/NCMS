using NCMS_Local.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NCMS_Local
{
    public class GOperator
    {
        public static int ID { get; set; }
        public static string Name { get; set; }
    }
    public static class GSettings
    {
        internal static string HisConnStr = @"Data Source=.;Initial Catalog=cbhis;Integrated Security=True";
        internal static string NhConnStr = @"Data Source=.;Initial Catalog=HNXT_interface;Integrated Security=True";
        public static IEnumerable<CDoctor> Doctors = null;
        public  static IEnumerable<CDepart> Departs = null;
        static GSettings()
        {
            HisComponent his=new HisComponent(HisConnStr);

            Doctors = his.GetDoctors();
        }

    }
}
