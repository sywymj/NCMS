using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace NCMS_Local.NHFUN
{
    public enum EnFamilyType
    {
        一般农合 = 1,
        五保户 = 2,
        贫困户 = 3,
        特困户 = 4,
        军烈属 = 5,
        其它 = 9
    }
    public enum EnYesOrNo
    {
        是=1,
        否=2
    }
    public enum EnManStatus
    {
        正常=1,
        迁入=2,
        迁出=3,
        死亡=4
    }
    public enum EnIsLocal
    {
        外地人,
        本地人
    }

    [TypeConverter(typeof(ConvertHrGetHzPersonInfo))]
    public class HrGetHzPersonInfo
    {
        public string coopMedCode { get; set; }
        public int aiIDNo { get; set; }
        public string areaCode { get; set; }
        public string name { get; set; }
        public string spellPy { get; set; }
        public string spellWb { get; set; }
        public string sex { get; set; }
        public string birthday { get; set; }
        public string address { get; set; }
        public string relationShipCode { get; set; }
        public string relationShipDesc { get; set; }
        public string psn { get; set; }
        public string operCode { get; set; }
        public string operName { get; set; }
        public EnFamilyType familyType { get; set; }
        public EnYesOrNo familyMaster { get; set; }
        public EnYesOrNo isSocial { get; set; }
        public EnManStatus manStatus { get; set; }
        public string changeDate { get; set; }
        public string socialYears { get; set; }
        public EnIsLocal isLocal { get; set; }
        public string bankAccount { get; set; }

        public static implicit operator HrGetHzPersonInfo(string val)
        {
            ConvertHrGetHzPersonInfo convertObj = new ConvertHrGetHzPersonInfo();
            return (HrGetHzPersonInfo)convertObj.ConvertFrom(val);
        }
    }
    public class ConvertHrGetHzPersonInfo:ExpandableObjectConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType==typeof(System.String))
            {
                return false;
            }
            return base.CanConvertTo(context, destinationType);
        }
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType==typeof(System.String))
            {
                return true;
            }
            return base.CanConvertFrom(context, sourceType);
        }
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value is string)
            {
                string[] temStrArray = value.ToString().Split(new string[] { "|" }, StringSplitOptions.None);
                return new HrGetHzPersonInfo()
                {
                    coopMedCode = temStrArray[0],
                    aiIDNo = int.Parse(temStrArray[1]),
                    areaCode = temStrArray[2],
                    name = temStrArray[3],
                    spellPy = temStrArray[4],
                    spellWb = temStrArray[5],
                    sex = temStrArray[6],
                    birthday = temStrArray[7],
                    address = temStrArray[8],
                    relationShipCode = temStrArray[9],
                    relationShipDesc = temStrArray[10],
                    psn = temStrArray[11],
                    operCode = temStrArray[12],
                    operName = temStrArray[13],
                    familyType = (EnFamilyType)int.Parse(temStrArray[14]),
                    familyMaster = (EnYesOrNo)int.Parse(temStrArray[15]),
                    isSocial = (EnYesOrNo)int.Parse(temStrArray[16]),
                    manStatus = (EnManStatus)int.Parse(temStrArray[17]),
                    changeDate = temStrArray[18],
                    socialYears = temStrArray[19],
                    isLocal = (EnIsLocal)int.Parse(temStrArray[20]),
                    bankAccount = temStrArray[21]
                };

            }
            return base.ConvertFrom(context, culture, value);
        }
    }
}
