using NCMS_Local.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NCMS_Local.DTOUI
{
    public enum DoctorSelEnum
    {
        医生=1,
        护师,
        行政人员,
        其它,
        后勤人员,
        收费人员,
        药剂,
        全部
    }
    public partial class DUIDoctorSel : Form
    {
        public DUIDoctorSel()
        {
            InitializeComponent();
        }
        public DoctorSelEnum doctorSelEnum = DoctorSelEnum.全部;
        public CDoctor SelDoctor;
        private void DUIDoctorSel_Load(object sender, EventArgs e)
        {

            this.dataGridView1.DataSource = (from dd in GSettings.Doctors where (dd.zglb == (int)doctorSelEnum)||doctorSelEnum==DoctorSelEnum.全部 select dd).ToArray();
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            {
                this.dataGridView1.DataSource = (from dd in GSettings.Doctors where (dd.zglb == (int)doctorSelEnum || doctorSelEnum == DoctorSelEnum.全部) && dd.pym.ToLower().Contains(this.textBox1.Text.Trim()) select dd).ToArray();
                
            }
        }

        private void dataGridView1_SizeChanged(object sender, EventArgs e)
        {
            try
            {
                SelDoctor = GSettings.Doctors.Where(dd => dd.zgdm == (int)this.dataGridView1.SelectedRows[0].Cells["zgdm"].Value).First();
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}
