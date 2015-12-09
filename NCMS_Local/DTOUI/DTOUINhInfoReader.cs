using NCMS_Local.NHFUN;
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
    public partial class DTOUINhInfoReader : Form
    {
        public DTOUINhInfoReader()
        {
            InitializeComponent();
        }

        List<AreaItem> areas = null;
        public object NhObj { get; set; }
        
        private void DTOUINhInfoReader_Load(object sender, EventArgs e)
        {
            areas = new List<AreaItem>();
            areas.Add(new AreaItem { areaID = "420302", areaDesc = "茅箭区" });
            areas.Add(new AreaItem { areaID = "420303", areaDesc = "张湾区" });
            areas.Add(new AreaItem { areaID = "420305", areaDesc = "武当山" });
            areas.Add(new AreaItem { areaID = "420321", areaDesc = "郧阳区" });
            areas.Add(new AreaItem { areaID = "420322", areaDesc = "郧西县" });
            areas.Add(new AreaItem { areaID = "420323", areaDesc = "竹山县" });
            areas.Add(new AreaItem { areaID = "420324", areaDesc = "竹溪县" });
            areas.Add(new AreaItem { areaID = "420325", areaDesc = "房县" });
            areas.Add(new AreaItem { areaID = "420371", areaDesc = "十堰经济技术开发区" });
            areas.Add(new AreaItem { areaID = "420381", areaDesc = "丹江口" });

            this.comboBox1.DataSource = areas;
        }

        private void buttonQuery_Click(object sender, EventArgs e)
        {
            string SelAreaID=((AreaItem)this.comboBox1.SelectedItem).areaID;
            StringBuilder sb = null;
            string CoopMedCode = string.Empty;
            int AiIDNo = -1;
            int hr = -1;
            string nhCodeID = this.textBox1.Text.Trim();
            object targetObj = null;


            if (string.IsNullOrEmpty(nhCodeID))
            {
                return;
            }
            try
            {
                if (SelAreaID == GSettings.OrganIDLocal)
                {
                    //本地农合
                    if (radioButtonKh.Checked)
                    {
                        //通过卡号获取农合证号
                        sb = new StringBuilder(256);
                        hr=NhLocalWrap.GetCoopMedCodeByCardID(GSettings.ParamLocalOrganID, nhCodeID, sb);
                        if (hr<0)
                        {
                            throw new Exception(sb.ToString());
                        }
                        CoopMedCode = sb.ToString().Split(new string[]{"$$"}, StringSplitOptions.None)[0];
                        AiIDNo =int.Parse(sb.ToString().Split(new string[] { "$$" }, StringSplitOptions.None)[1]) ;
                    }
                    sb = new StringBuilder(256);
                    hr = NhLocalWrap.GetHzPersonInfo(GSettings.ParamLocalOrganID, CoopMedCode, sb);
                    if (hr < 0)
                    {
                        throw new Exception(sb.ToString());
                    }
                    targetObj = (HrGetHzPersonInfo)sb.ToString();
                    
                }
                else
                {
                    //异地农合
                    if (radioButtonKh.Checked)
                    {
                        //通过卡号获取农合号
                        sb = new StringBuilder(256);
                        hr = NhLocalWrap.zzGetCoopMedCodeByCardID(GSettings.ParamRemoteOrganID,SelAreaID, nhCodeID, sb);
                        if (hr < 0)
                        {
                            throw new Exception(sb.ToString());
                        }
                        CoopMedCode = sb.ToString().Split(new string[] { "$$" }, StringSplitOptions.None)[0];
                        AiIDNo = int.Parse(sb.ToString().Split(new string[] { "$$" }, StringSplitOptions.None)[1]);
                    }
                    sb = new StringBuilder(256);
                    hr = NhLocalWrap.GetZzinfo_zz(
                        string.Format(@"{0}$${1}", "1", GSettings.AccountYear),
                        string.Format("{0}$${1}$${2}$${3}", GSettings.OrganIDRemote, SelAreaID, CoopMedCode, AiIDNo),
                        sb
                        );
                    if (hr < 0)
                    {
                        throw new Exception(sb.ToString());
                    }

                    targetObj =(HrGetZzinfo_zz) sb.ToString();
                }

                this.propertyGrid1.SelectedObject = targetObj;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void toolStripButtonOK_Click(object sender, EventArgs e)
        {
            NhObj = this.propertyGrid1.SelectedObject;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void toolStripButtonCancel_Click(object sender, EventArgs e)
        {
            this.NhObj = null;
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
    struct AreaItem
    {
        public string areaID { get; set; }
        public string areaDesc { get; set; }
        public override string ToString()
        {
            return string.Format(@"{0}  {1}", areaID, areaDesc);
        }
    }
}
