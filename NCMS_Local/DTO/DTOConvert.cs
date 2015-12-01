using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Windows.Forms.Design;

namespace NCMS_Local.DTO
{
    public class ConvertSex : StringConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
        public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new TypeConverter.StandardValuesCollection(new string[] { "男", "女" });
        }
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return true;
            //return base.GetStandardValuesExclusive(context);
        }
    }
    public class ConvertMarray : StringConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
        public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new TypeConverter.StandardValuesCollection(new string[] { "未婚", "已婚","离婚","丧偶" });
        }
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return true;
        }
    }
    public class ConvertAgeUnit : StringConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
        public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new TypeConverter.StandardValuesCollection(new string[] { "岁", "月", "天"});
        }
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return true;
        }
    }
    public class ConvertAge :ExpandableObjectConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType==typeof(System.String) && value is CAge)
            {
                CAge obj = (CAge)value;
                return string.Format(@"{0}{1}", obj.Age, obj.Unit);
            }
            return base.ConvertTo(context, culture, value, destinationType);
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
                string str=value as string;
                CAge obj = new CAge();
                obj.Unit = str.Substring(str.Length - 1, 1);
                obj.Age = int.Parse(str.Substring(0, str.Length - 1));

                return obj;
            }
            return base.ConvertFrom(context, culture, value);
        }
    }
    public class ConvertDoctor : ExpandableObjectConverter
    {
        
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return base.CanConvertTo(context, destinationType);
        }
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(System.String) && value is CDoctor)
            {
                CDoctor obj = (CDoctor)value;
                return string.Format(@"{0}{1}", obj.bm.bmdm, obj.zgmc);
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
    public class MzDoctorEdit : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (edSvc != null)
            {
                DTOUI.DUIDoctorSel ds = new DTOUI.DUIDoctorSel();
                ds.doctorSelEnum = DTOUI.DoctorSelEnum.医生;
                edSvc.ShowDialog(ds);
                return ds.SelDoctor;
            }
            return value;
        }
    }

    public class NhIllEdit : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (edSvc != null)
            {
                DTOUI.DUIIllSel ds = new DTOUI.DUIIllSel();
                
                edSvc.ShowDialog(ds);
                return ds.SelIll;
            }
            return value;
        }
    }
}
