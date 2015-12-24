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
            HisCom = new HisComponent();
        }
    }
}
