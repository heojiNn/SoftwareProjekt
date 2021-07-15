using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using XCV.Data;

namespace Tests.Integration
{

    //----------------------Integration Tests for    IOfferService
    [TestFixture]
    class IOfferService_Test : Initializer
    {

        private OfferService sut;

        [OneTimeSetUp]
        public void GetSut()
        {
            sut = GetOfferService();
        }





















    }
}
