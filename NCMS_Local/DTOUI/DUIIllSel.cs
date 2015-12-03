using NCMS_Local.Component;
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
    public partial class DUIIllSel : Form
    {
        public DUIIllSel()
        {
            InitializeComponent();
        }
        private NhComponent NhDb = null;
        public CIll SelIll;
        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            {
                this.dataGridView1.DataSource = NhDb.GetIllsByPym(this.textBox1.Text.Trim());
            }
        }

        private void DUIIllSel_Load(object sender, EventArgs e)
        {
            NhDb = new NhComponent();
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                this.SelIll=NhDb.GetIllsByIllCode(this.dataGridView1.SelectedRows[0].Cells["IllCode"].Value.ToString());
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
