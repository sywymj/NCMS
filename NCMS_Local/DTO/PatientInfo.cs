using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace NCMS_Local.DTO
{

    public class CAge
    {
        public int Age{ get; set; }

        [TypeConverter(typeof(ConvertAgeUnit)),DisplayName("年龄单位")]
        public string Unit { get; set; }
        public CAge()
        {
            this.Unit = "岁";
        }
    }
    
    public class PatientInfo:INotifyPropertyChanged
    {
        private CAge _oAge;
        private DateTime? _birthday;

        [Category("基本信息")]
        [DisplayName("姓名")]
        public string Name { get; set; }

        [Browsable(false)]
        public short? Age { get; set; }
        [Browsable(false)]
        public string AgeUnit { get; set; }

        [Category("基本信息")]
        [DisplayName("年龄")]
        [TypeConverter(typeof(ConvertAge))]
        public CAge oAge
        {
            get { return this._oAge; }
            set
            {
                if (value!=this._oAge)
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

        [Category("基本信息")]
        [DisplayName("出生日期")]
        public DateTime? BirthDay
        {
            get { return _birthday; }
            set
            {
                if (this._birthday!=value)
                {
                    this._birthday = value;
                    OnPropertyChanged("BirthDay");
                }
            }
        }

        [Category("基本信息")]
        [DisplayName("婚姻状况")]
        [TypeConverter(typeof(ConvertMarray))]
        public string Marray { get; set; }

        [Category("基本信息")]
        [DisplayName("性别")]
        [TypeConverter(typeof(ConvertSex))]
        public string Sex { get; set; }

        [Category("基本信息")]
        [DisplayName("身份证号")]
        public string PSN { get; set; }

        public int? HisZyh { get; set; }
        public int? HisRyh
        {
            get { return HisZyh; }
            set { HisZyh = value; }
        }
        public short? HisNationCode { get; set; }
        public string HisNation { get; set; }

        public short? HisRyksCode { get; set; }
        public string HisRyksDesc{get;set;}
        public int? HisZybrlx { get; set; }
        public DateTime? Ryrq { get; set; }
        public int? HisCzyCode { get; set; }

        public string HisMzzdCode { get; set; }
        public string HisMzzdDesc { get; set; }
        public string HisRyzdCode{get;set;}
        public string HisRyzdDesc{get;set;}
        public short? HisDoctorCode { get; set; }
        public string HisDoctorName { get; set; }
        public short? HisMzDoctorCode { get; set; }
        public string HisMzDoctorName { get; set; }

        public PatientInfo()
        {
            this.oAge = new CAge();
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
