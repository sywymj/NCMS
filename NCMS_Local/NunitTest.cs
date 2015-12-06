﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NCMS_Local.DTO;

namespace NCMS_Local
{
    [TestFixture]
    public class NunitTest
    {
        private string _connStr="Data Source=.;Initial Catalog=cbhis;Integrated Security=True";

        private void pt(PatientInfo pi,int i,string s)
        {
            pi.Name="majuan";
            i = 1000;
            s = "hello world";
        }
        [Test]
        [Path]
        public void InpatientRegister()
        {
            

            HisComponent hisComponent = new HisComponent();

            PatientInfo pinfo = new PatientInfo();
            
            try
            {
                //int hr=hisComponent.InpatientRegister(pinfo);
                //Console.WriteLine(hr);
                //Assert.AreNotEqual(-1, hr);
                int hr=hisComponent.JzdToNhFeeListByZyh(45094);
                List<string> ls = (List<string>)hisComponent.ProcessFeeListByZyh(45094,false);
                Assert.AreEqual(0, hr);
            }
            catch (System.Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
    }
}
