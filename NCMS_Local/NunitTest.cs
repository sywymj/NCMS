using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NCMS_Local.DTO;
using System.IO;

namespace NCMS_Local
{
    [TestFixture]
    public class NunitTest
    {
        [TestFixtureSetUp]
        public void InitWorkDirectory()
        {
            Directory.SetCurrentDirectory(TestContext.CurrentContext.TestDirectory);
        }
        [Test]
        [Ignore("c")]
        public void ClearFee()
        {
            HisComponent hisComponent = new HisComponent();
            hisComponent.ClearAllUploadedFeeByZyh(45094);
        }
        [Test]
        [Ignore("c")]
        public void UpLoadFee()
        {
            HisComponent hisComponent = new HisComponent();
            int iHr=hisComponent.JzdToNhFeeListByZyh(45094);
            Assert.AreEqual(0, iHr);
            var errors=hisComponent.ProcessFeeListByZyh(45094, true);
            Assert.AreEqual(errors.Count(), 0);
        }

        [Test]
        //取消结算
        public void CancelCal()
        {
            HisComponent hisComponent = new HisComponent();
            hisComponent.HisBalanceDel(45094);
        }
        //费用上传测试
        [Test]
        public void InpatientRegister()
        {
            HisComponent hisComponent = new HisComponent();
            try
            {
                //string hr = hisComponent.HisBalanceDel(45094);
                //Assert.AreEqual(string.Empty, hr);

                ParamBalance pbalance = new ParamBalance() { zyh = 45094, outDate = DateTime.Now };
                string hr = hisComponent.HisBalance(pbalance);
                Assert.AreEqual(string.Empty, hr);
   
                //int hr=hisComponent.JzdToNhFeeListByZyh(45094);
                //Assert.AreEqual(0, hr);
                //List<string> ls = (List<string>)hisComponent.ProcessFeeListByZyh(45094,true);
                //Assert.AreEqual(0,ls.Count);
            }
            catch (System.Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
    }
}
