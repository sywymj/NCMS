using NCMS_Local;
using NCMS_Local.DTOUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace NCMS_Win
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {

            HisComponent hisComponent = new HisComponent();

            //上传费用
            int hr = hisComponent.JzdToNhFeeListByZyh(45094);
            List<string> ls = (List<string>)hisComponent.ProcessFeeListByZyh(45094, true);

            //清除所有费用；
            //string hr = hisComponent.ClearAllUploadedFeeByZyh(45094);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            //Application.Run(new NCMS_Local.UI.FormInpatientRegister());

            //DUIDoctorSel dsForm = new DUIDoctorSel();
            //Application.Run(new NCMS_Local.DTOUI.DUIDoctorSel());
            //Console.WriteLine(dsForm.SelDoctor.zgmc);
            //DTOUINhInfoReader nhReader = new DTOUINhInfoReader();
            //Application.Run(nhReader);
        }
    }
}
