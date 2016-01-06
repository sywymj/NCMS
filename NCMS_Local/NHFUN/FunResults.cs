using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace NCMS_Local.NHFUN
{
    public enum EnFamilyType
    {
        一般农户 = 1,
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


    public class NhPersonInfoBase
    {
        [Category("基本信息"),DisplayName("农合证号")]
        public string coopMedCode { get; set; }

        [Browsable(false)]
        public int aiIDNo { get; set; }


        [Category("基本信息"),DisplayName("区域代码")]
        public string areaCode { get; set; }
        [Category("基本信息"),DisplayName("姓名")]
        public string name { get; set; }
        [Category("基本信息"),DisplayName("性别")]
        public string sex { get; set; }
        [Category("基本信息"),DisplayName("出生日期")]
        public string birthday { get; set; }
        [Category("基本信息"), DisplayName("身份证号")]
        public string psn { get; set; }
        [Category("基本信息"), DisplayName("家庭地址")]
        public string address { get; set; }

        public override string ToString()
        {
            if (areaCode.StartsWith("420302"))
            {
                return string.Format(@"本地农合：{0}  {1}", name, coopMedCode);
            } 
            else
            {
                return string.Format(@"异地农合：{0}  {1}", name, coopMedCode);
            }
        }
        [Browsable(false)]
        public string FunHrStr { get; set; }
    }
    public class HrGetZzinfo_zz : NhPersonInfoBase
    {
        [DisplayName("转诊序号")]
        public int transfNo { get; set; }
        [DisplayName("转诊疾病编码")]
        public string illCode { get; set; }
        [DisplayName("转诊疾病名称")]
        public string illDesc { get; set; }
        [DisplayName("转诊前医院")]
        public string preHosp { get; set; }
        [DisplayName("转诊原因")]
        public string transfCase { get; set; }
        [Browsable(false)]
        public string approveOpinio { get; set; }
        [DisplayName("转诊部门")]
        public string approveDepart { get; set; }
        [DisplayName("转诊时间")]
        public string approveDate { get; set; }

        public static implicit operator HrGetZzinfo_zz(string value)
        {
            try
            {
                string[] temStrArray = value.ToString().Split(new string[] { "|" }, StringSplitOptions.None);
                return new HrGetZzinfo_zz()
                {
                    FunHrStr=value,
                    coopMedCode = temStrArray[0],
                    name = temStrArray[1],
                    aiIDNo = int.Parse(temStrArray[2]),
                    areaCode = temStrArray[3],
                    transfNo = int.Parse(temStrArray[4]),
                    illCode = temStrArray[5],
                    illDesc = temStrArray[6],
                    preHosp = temStrArray[7],
                    transfCase = temStrArray[8],
                    approveOpinio = temStrArray[9],
                    approveDepart = temStrArray[10],
                    approveDate = temStrArray[11],
                    psn = temStrArray[12],
                    birthday = temStrArray[13],
                    sex = temStrArray[14],
                    address = temStrArray[15]
                };
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

        }
    }
    public class HrGetHzPersonInfo:NhPersonInfoBase
    {
        [Browsable(false)]
        public string spellPy { get; set; }
        [Browsable(false)]
        public string spellWb { get; set; }
        
        
        [Browsable(false)]
        public string relationShipCode { get; set; }
        [Browsable(false)]
        public string relationShipDesc { get; set; }
        
        [Browsable(false)]
        public string operCode { get; set; }
        [Browsable(false)]
        public string operName { get; set; }
        [DisplayName("户属性")]
        public EnFamilyType familyType { get; set; }
        [Browsable(false)]
        public EnYesOrNo familyMaster { get; set; }
       
        [DisplayName("是否参合")]
        public EnYesOrNo isSocial { get; set; }
        [DisplayName("人员状态")]
        public EnManStatus manStatus { get; set; }
        [Browsable(false)]
        public string changeDate { get; set; }
        [Browsable(false)]
        public string socialYears { get; set; }
         [Browsable(false)]
        public EnIsLocal isLocal { get; set; }
        [Browsable(false)]
        public string bankAccount { get; set; }

        public static implicit operator HrGetHzPersonInfo(string value)
        {
            try
            {
                string[] temStrArray = value.ToString().Split(new string[] { "|" }, StringSplitOptions.None);
                return new HrGetHzPersonInfo()
                {
                    FunHrStr=value,
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
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
            
        }
    }

    public class HrPreClearing
    {
        public decimal TotalFee { get; set; }
        public decimal ReimRangeFee { get; set; }
        public decimal ReimFee { get; set; }
        public decimal HospitalReduction { get; set; }
        public decimal CivilPay { get; set; }
        public decimal ScAmount { get; set; }
        public decimal HospitalCost { get; set; }
        public decimal BeginLimite { get; set; }
        public string Retain1 { get; set; }
        public decimal SpecialIllHospitalCost { get; set; }
        public string Retain2 { get; set; }
        public decimal YearLimite { get; set; }
        public decimal YearTotalReimFee { get; set; }

        public static implicit operator HrPreClearing(string value)
        {
            try
            {
                string[] temStrArray = value.ToString().Split(new string[] { "|" }, StringSplitOptions.None);
                return new HrPreClearing()
                {
                    TotalFee = Convert.ToDecimal(temStrArray[0]),
                    ReimRangeFee = Convert.ToDecimal(temStrArray[1]),
                    ReimFee = Convert.ToDecimal(temStrArray[2]),
                    HospitalReduction = Convert.ToDecimal(temStrArray[3]),
                    CivilPay = Convert.ToDecimal(temStrArray[4]),
                    ScAmount = Convert.ToDecimal(temStrArray[5]),
                    HospitalCost = Convert.ToDecimal(temStrArray[6]),
                    BeginLimite = Convert.ToDecimal(temStrArray[7]),
                    SpecialIllHospitalCost = Convert.ToDecimal(temStrArray[9]),
                    YearLimite = Convert.ToDecimal(temStrArray[11]),
                    YearTotalReimFee = Convert.ToDecimal(temStrArray[12])
                };
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
