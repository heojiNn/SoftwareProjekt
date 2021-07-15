using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using XCV.Data;

namespace Tests.Integration
{
    [TestFixture]
    class IConfigService_Test : Initializer
    {

        private IConfigService sut;

        [OneTimeSetUp]
        public void GetSut()
        {
            sut = GetConfigService();
        }










    }
}
