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
        
        //费用上传测试
        [Test]
        public void InpatientRegister()
        {
            HisComponent hisComponent = new HisComponent();
            try
            {
                string hr=hisComponent.HisBalance(45384);

   
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
