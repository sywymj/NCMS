using NCMS_Local.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NCMS_Local
{
    public enum NhType
    {
        Local,
        Remote
    }
    

    public static class GSettings
    {
        //internal static string HisConnStr = @"Data Source=.;Initial Catalog=cbhis;Integrated Security=True";
        //internal static string HisConnStr = @"Data Source=192.0.2.2;Initial Catalog=wycs;uid=sa;pwd=11003";
        internal static string HisConnStr = @"Data Source=192.0.2.2;Initial Catalog=wycs;uid=sa;pwd=11003";
        //internal static string NhConnStr = @"Data Source=.;Initial Catalog=HNXT_interface;Integrated Security=True";
        internal static string NhConnStr = @"Data Source=192.0.2.3;Initial Catalog=HNXT_interface;uid=sa;pwd=11003";
        public static IEnumerable<CDoctor> Doctors = null;
        public  static IEnumerable<CDepart> Departs = null;

        public static string OrganIDLocal = "420302";
        public static string OrganIDRemote = "420000";
        public static string AccountYear = "2015";
        public static short OperatorID = 74;
        public static string OperatorName = "刘霞";
        public static string AdLimitDef = "0";

        public static string ParamLocalOrganID
        {
            get
            {
                return GetOrganIDParam(NhType.Local);
            }
        }
        public static string ParamRemoteOrganID
        {
            get
            {
                return GetOrganIDParam(NhType.Remote);
            }
        }


        private static string GetOrganIDParam(NhType nhType)
        {
            return string.Format(@"{1}$${0}", AccountYear, nhType == NhType.Local ? OrganIDLocal : OrganIDRemote);
        }
        static GSettings()
        {
            HisComponent his=new HisComponent();

            Doctors = his.GetDoctors();
        }

    }
}
