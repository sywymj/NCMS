using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NCMS_Local.DTO;
using NCMS_Local;

namespace NCMS_Win
{
    public partial class FormMaster : Form
    {
        public FormMaster()
        {
            InitializeComponent();
        }

        HisComponent HisCom = null;

        private void toolStripButtonNewReg_Click(object sender, EventArgs e)
        {
            PatientInfo pInfo = new PatientInfo();
            pInfo.HisZyh = HisCom.MakeZyh();
            this.propertyGridPatientInfo.SelectedObject = pInfo;
        }

        private void FormMaster_Load(object sender, EventArgs e)
        {
            try
            {
                HisCom = new HisComponent();
                StringBuilder sb = new StringBuilder(256);
                int hr = NhLocalWrap.InitDLL(sb);
                if (hr < 0)
                {
                    throw new Exception(sb.ToString());
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            

        }

        private void toolStripButtonSaveReg_Click(object sender, EventArgs e)
        {
            PatientInfo pInfo = this.propertyGridPatientInfo.SelectedObject as PatientInfo;
            if (pInfo==null)
            {
                return;
            }
            if (MessageBox.Show("确定保存当前入院登记信息吗？","提示",MessageBoxButtons.OKCancel,MessageBoxIcon.Question)!=DialogResult.OK)
            {
                return;
            }

            int _zyh = -1;
            Guid _nhGuid = Guid.Empty;
            try
            {
                _zyh=HisCom.NewPatientRegister(pInfo);
                if (pInfo.NhInfo!=null)
                {
                    _nhGuid = HisCom.NewNhRegister(pInfo);
                }
                
                MessageBox.Show("入院登记成功\r\n住院号：" + _zyh.ToString());
                
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


            

        }
    }
}
