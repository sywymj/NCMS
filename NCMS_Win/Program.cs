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
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
            //Application.Run(new NCMS_Local.UI.FormInpatientRegister());

            //DUIDoctorSel dsForm = new DUIDoctorSel();
            //Application.Run(new NCMS_Local.DTOUI.DUIDoctorSel());
            //Console.WriteLine(dsForm.SelDoctor.zgmc);
            DTOUINhInfoReader nhReader = new DTOUINhInfoReader();
            Application.Run(nhReader);
        }
    }
}
