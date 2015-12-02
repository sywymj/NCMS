using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace NCMS_Local
{
    public class NhLocalWrap
    {
        [DllImport("LxClient.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int InitDLL([MarshalAs(UnmanagedType.LPStr)]StringBuilder dataBuf);

        /// <summary>
        /// 根据卡号获取农合证号
        /// </summary>
        /// <param name="aOrganID"></param>
        /// <param name="aCardID"></param>
        /// <param name="dataBuf"></param>
        /// <returns></returns>
        [DllImport("LxClient.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int GetCoopMedCodeByCardID([MarshalAs(UnmanagedType.LPStr)]string aOrganID, [MarshalAs(UnmanagedType.LPStr)] string aCardID, [MarshalAs(UnmanagedType.LPStr)]StringBuilder dataBuf);

        /// <summary>
        /// 转诊：：根据卡号获取农合证号
        /// </summary>
        /// <param name="aOrganID"></param>
        /// <param name="aAreaCode"></param>
        /// <param name="aCardID"></param>
        /// <param name="dataBuf"></param>
        /// <returns></returns>
        [DllImport("LxClient.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int zzGetCoopMedCodeByCardID([MarshalAs(UnmanagedType.LPStr)]string aOrganID,[MarshalAs(UnmanagedType.LPStr)]string aAreaCode, [MarshalAs(UnmanagedType.LPStr)] string aCardID, [MarshalAs(UnmanagedType.LPStr)]StringBuilder dataBuf);

        /// <summary>
        /// 获取参合人鱼信息
        /// </summary>
        /// <param name="AsOrganID"></param>
        /// <param name="农合证号"></param>
        /// <param name="dataBuf"></param>
        /// <returns></returns>
        [DllImport("LxClient.dll", CharSet = CharSet.Auto,CallingConvention=CallingConvention.StdCall)]
        public static extern int GetHzPersonInfo([MarshalAs(UnmanagedType.LPStr)]string AsOrganID,[MarshalAs(UnmanagedType.LPStr)] string AsCoopMedCode, [MarshalAs(UnmanagedType.LPStr)]StringBuilder dataBuf);

        /// <summary>
        /// 入院登记||住院信息修改
        /// </summary>
        /// <param name="AsOrganID"></param>
        /// <param name="AsCoopMedCode"></param>
        /// <param name="AsExpressionsID"></param>
        /// <param name="AsPatientName"></param>
        /// <param name="AiIDNo"></param>
        /// <param name="AsIllCode"></param>
        /// <param name="AsIllName"></param>
        /// <param name="AInDate"></param>
        /// <param name="Adke"></param>
        /// <param name="AdLimitDef"></param>
        /// <param name="AsDoctor"></param>
        /// <param name="AsPatientId"></param>
        /// <param name="AsFlag"></param>
        /// <param name="AiDiagNo"></param>
        /// <param name="AsExpenseKind"></param>
        /// <param name="AsLimitIllCode"></param>
        /// <param name="dataBuf"></param>
        /// <returns></returns>
        [DllImport("LxClient.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int SaveInHosInfo([MarshalAs(UnmanagedType.LPStr)]string AsOrganID, [MarshalAs(UnmanagedType.LPStr)]string AsCoopMedCode, [MarshalAs(UnmanagedType.LPStr)]string AsExpressionsID, [MarshalAs(UnmanagedType.LPStr)]string AsPatientName, int AiIDNo,
    [MarshalAs(UnmanagedType.LPStr)]string AsIllCode, [MarshalAs(UnmanagedType.LPStr)]string AsIllName, [MarshalAs(UnmanagedType.LPStr)]string AInDate, [MarshalAs(UnmanagedType.LPStr)]string Adke, [MarshalAs(UnmanagedType.LPStr)]string AdLimitDef, [MarshalAs(UnmanagedType.LPStr)]string AsDoctor, [MarshalAs(UnmanagedType.LPStr)]string AsPatientId, [MarshalAs(UnmanagedType.LPStr)]string AsFlag, [MarshalAs(UnmanagedType.LPStr)]string AiDiagNo, [MarshalAs(UnmanagedType.LPStr)]string AsExpenseKind,
          [MarshalAs(UnmanagedType.LPStr)]string AsLimitIllCode, [MarshalAs(UnmanagedType.LPStr)]StringBuilder dataBuf);

        /// <summary>
        /// 4.1.18.[住院] 取消入院登记（包括费用明细）
        /// </summary>
        /// <param name="AsOrganID"></param>
        /// <param name="AsCoopMedCode"></param>
        /// <param name="AiIDNo"></param>
        /// <param name="AiDiagNo"></param>
        /// <param name="dataBuf"></param>
        /// <returns></returns>
        [DllImport("LxClient.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int DeleteHosInfo([MarshalAs(UnmanagedType.LPStr)]string AsOrganID, [MarshalAs(UnmanagedType.LPStr)]string AsCoopMedCode, int AiIDNo, int AiDiagNo, [MarshalAs(UnmanagedType.LPStr)]StringBuilder dataBuf);

        [DllImport("LxClient.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int GetZzinfo_zz([MarshalAs(UnmanagedType.LPStr)]string aGrade, [MarshalAs(UnmanagedType.LPStr)]string aAreaCode, [MarshalAs(UnmanagedType.LPStr)]StringBuilder dataBuf);

    }
}
