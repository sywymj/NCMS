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

        public static string OrganIDLocal = "420302";
        public static string OrganIDRemote = "420000";
        public static string AccountYear = "2015";

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
            HisComponent his=new HisComponent(HisConnStr);

            Doctors = his.GetDoctors();
        }

    }
}
