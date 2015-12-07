using NCMS_Local.NHFUN;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;

namespace NCMS_Local.DTO
{
    public enum EnumExpenseKind
    {
        普通住院=21,
        单病种住院=22,
        正常分娩住院=23,
        其他住院=29,
        其他=90
    }
    public enum EnumZybxGS
    {
        二级医院=2,
        低保五保二级医院=10,
        产后并发症或合并症=13,
        跨区域结算县级=63
    }
    public enum EnumNation
    {
        汉族=1,
        苗族=2,
        彝族=3
    }
    public enum EnumRyLb
    {
        普通病人=1,
        农村合作医疗病人=9
    }
    public struct CNhBaseInfo
    {
        public string OrganID { get; set; }
        public string OrganDesc { get; set; }
        public string CoopMedCode { get; set; }
        public int AiIDNo { get; set; }
        public int TurnID { get; set; }
        public string Name { get; set; }
    }
    

    public struct CDepart
    {
        [DisplayName("ID")]
        public int bmdm { get; set; }
        [DisplayName("科室名称")]
        public string bmmc { get; set; }
        [Browsable(false)]
        public string pym { get; set; }
        [Browsable(false)]
        public int kslb { get; set; }
        public override string ToString()
        {
            return bmmc;
        }
    }
    public struct CDoctor
    {
        [DisplayName("ID")]
        public int zgdm { get; set; }
        [DisplayName("医师名称")]
        public string zgmc { get; set; }
        [DisplayName("科别")]
        public CDepart bm { get; set; }
        [Browsable(false)]
        public string pym { get; set; }
        [Browsable(false)]
        public int zglb { get; set; }

        public override string ToString()
        {
            return string.Format("{1}  {0}", zgmc, bm.bmmc);
        }
    }
    public struct CIll
    {
        [DisplayName("疾病编码")]
        public string IllCode { get; set; }
         [DisplayName("疾病名称")]
        public string IllDesc { get; set; }
        [Browsable(false)]
        public string Spell { get; set; }
        public override string ToString()
        {
            return string.Format(@"{0}  {1}", IllCode, IllDesc);
        }
    }
    public struct CAge
    {
        public int Age{ get; set; }

        [TypeConverter(typeof(ConvertAgeUnit)),DisplayName("年龄单位")]
        public string Unit { get; set; }
        
    }
    
    public class PatientInfo:INotifyPropertyChanged
    {
        private CAge _oAge;
        private DateTime? _birthday;
        private NhPersonInfoBase _nhInfo;

        [Category("\t\t基本信息"),DisplayName("姓名")]
        public string Name { get; set; }

        [Browsable(false)]
        public short? Age { get; set; }
        [Browsable(false)]
        public string AgeUnit { get; set; }

        [Category("\t\t基本信息"), DisplayName("年龄")]
        [TypeConverter(typeof(ConvertAge))]
        [RefreshProperties(RefreshProperties.All)]
        public CAge oAge
        {
            get { return this._oAge; }
            set
            {
                if (!value.Equals(this._oAge))
                {
                    this._oAge = value;
                    switch (value.Unit)
                    {
                    case "岁":
                            this.BirthDay = DateTime.Now.AddYears(-1 * value.Age);
                    	break;
                    case "月":
                        this.BirthDay = DateTime.Now.AddMonths(-1 * value.Age);
                        break;
                    case "天":
                        this.BirthDay = DateTime.Now.AddDays(-1 * value.Age);
                        break;
                    }
                    OnPropertyChanged("oAge");
                }
            }
        }

        [Category("\t\t基本信息"),DisplayName("出生日期"),ReadOnly(true)]
        public DateTime? BirthDay
        {
            get { return _birthday; }
            set
            {
                if (this._birthday!=value)
                {
                    this._birthday = value;
                }
            }
        }

        [Category("\t\t基本信息"),DisplayName("婚姻状况")]
        [TypeConverter(typeof(ConvertMarray))]
        public string Marray { get; set; }

        [Category("\t\t基本信息")]
        [DisplayName("性别")]
        [TypeConverter(typeof(ConvertSex))]
        public string Sex { get; set; }

        [Category("\t\t基本信息")]
        [DisplayName("身份证号")]
        public string PSN { get; set; }

        

        [Category("\t\t基本信息")]
        [DisplayName("民族")]
        public EnumNation HisNationCode { get; set; }
        

        //[Category(""),DisplayName("")]
        [Category("\t入院登记信息"), DisplayName("住院号"),RefreshProperties(RefreshProperties.All)]
        public int? HisZyh { get; set; }
        [Browsable(false)]
        public int? HisRyh
        {
            get { return HisZyh; }
            set { HisZyh = value; }
        }

        [Category("\t入院登记信息"), DisplayName("患者类别")]
        public EnumRyLb HisZybrlx { get; set; }
        [Category("\t入院登记信息"), DisplayName("入院时间")]
        public DateTime? Ryrq { get; set; }
        [Category("\t入院登记信息"), DisplayName("门诊医生")]
        [Editor(typeof(MzDoctorEdit),typeof(UITypeEditor))]
        public CDoctor oMzDoctor { get; set; }
        [Category("\t入院登记信息"), DisplayName("住院医生")]
        [Editor(typeof(MzDoctorEdit), typeof(UITypeEditor))]
        public CDoctor oZyDoctor { get; set; }
        [Category("\t入院登记信息"), DisplayName("入院诊断")]
        [Editor(typeof(NhIllEdit),typeof(UITypeEditor))]
        public CIll oRyIll { get; set; }

        
        [Category("农合信息"), DisplayName("参保信息")]
        [Editor(typeof(NhInforReaderEdit), typeof(UITypeEditor))]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public NhPersonInfoBase NhInfo
        {
            get { return _nhInfo; }
            set
            {
                if (value!=_nhInfo)
                {
                    _nhInfo = value;
                    if (string.IsNullOrEmpty(Name) && value is NhPersonInfoBase)
                    {
                        NhPersonInfoBase oo = _nhInfo as NhPersonInfoBase;
                        this.Name = oo.name;
                        this.Sex = oo.sex;
                        this.BirthDay = DateTime.Parse(oo.birthday);
                        this.PSN = oo.psn;
                        this.HisZybrlx = EnumRyLb.农村合作医疗病人;
                        if (_nhInfo is HrGetZzinfo_zz)
                        {
                            this.NhZybxgs = EnumZybxGS.跨区域结算县级;
                        }
                        else
                        {
                            this.NhZybxgs = EnumZybxGS.二级医院;
                        }
                    }
                }
            }
        }
        [Category("农合信息"), DisplayName("住院报销公式")]
        public EnumZybxGS NhZybxgs { get; set; }
        [Category("农合信息"), DisplayName("补偿类别")]
        public EnumExpenseKind NhExpenseKind { get; set; }

        public PatientInfo()
        {
            this.oAge = new CAge() { Unit="岁"};
            this.Ryrq = DateTime.Now;
            this.HisZybrlx = EnumRyLb.普通病人;
            this.NhZybxgs = EnumZybxGS.二级医院;
            this.HisNationCode = EnumNation.汉族;
            this.NhExpenseKind = EnumExpenseKind.普通住院;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    
}
