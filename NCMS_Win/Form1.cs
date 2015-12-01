using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NCMS_Local.LTSQL;
using NCMS_Local;
namespace NCMS_Win
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private static string connStr = @"Data Source=.;Initial Catalog=cbhis;Integrated Security=True";
        NCMS_Local.DTO.PatientInfo pInfo = null;
        private void Form1_Load(object sender, EventArgs e)
        {
            //DCCbhisDataContext db = new DCCbhisDataContext();
            //var zgs = from b in db.ZG
            //          where b.ZGLBDM == 1
            //          select new
            //          {
            //              b.PYM,
            //              b.ZGXM,
            //              b.BM
            //          };
            //DataTable dtZgs=zgs.ToDataTable();
            pInfo = new NCMS_Local.DTO.PatientInfo();
            this.propertyGrid1.SelectedObject = pInfo;
            //this.propertyGrid1.SelectedObject = new CustomClass();
            //propertyGrid1.ExpandAllGridItems();
            this.dataGridView1.DataSource = GSettings.Doctors;
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }

    public class innerClass
    {

        public string a1 { get; set; }
        public string a2 { get; set; }
        public override string ToString()
        {
            return "i Lover you";
        }
    }
    public class CustomClass
    {
        public int iVal { get; set; }

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public innerClass nClass { get; set; }
        public CustomClass()
        {
            this.nClass = new innerClass();
        }
    }
    
    
}
