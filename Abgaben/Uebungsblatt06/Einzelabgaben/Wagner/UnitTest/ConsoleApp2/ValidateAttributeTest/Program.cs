using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using NUnit.Framework;

namespace UnitTestLibrary
{
    [TestFixture]
    class TestProgram
    {
        
        public static void Main(String[] args)
        {
            //üöäß

            string input = "üöäßüöäßüöäß     !!!!!!!!!!!!!!    IAUSFIGFIUGFUIDGFUSGIUFGD ";
            string regex = @"^[A-Z\u00c4\u00e4\u00d6\u00f6\u00dc\u00fc\u00df\s]+$";
            Regex regx = new Regex(regex);


            if (!RegexFormatCheckerTest(input))
            {
                Console.WriteLine("It does not match!!!");
            }
            else
            {
                Console.WriteLine("it matched");
            }
            Console.ReadLine();

        }


        [Test]
        private static bool RegexFormatCheckerTest(string str)
        {

            if (str == null) return false;
            Assert.IsNotNull(str, "Null value input");
            Regex rgx = new Regex(@"^[A-Z\u00c4\u00e4\u00d6\u00f6\u00dc\u00fc\u00df\s\u0021]+$");
            bool checker = rgx.IsMatch(str);

            /*
            Assert.IsFalse(rgx.IsMatch("Selbstorganisation!"), "Selbstorganisation! falsely assumed true");
            Assert.IsFalse(rgx.IsMatch("Team-Fähigkeit"), "Team-Fähigkeit falsely assumed true");

            Assert.IsTrue(rgx.IsMatch("Selbstorganisation"), "Selbstorganisation falsely assumed false");
            Assert.IsTrue(rgx.IsMatch("Teamfähigkeit"), "Teamfähigkeit falsely assumed false");
            Assert.IsTrue(rgx.IsMatch("Team"), "Team falsely assumed false");
            Assert.IsTrue(rgx.IsMatch("Mitarbeiterkoordination"), "Mitarbeiterkoordination falsely assumed false");
            */

            return checker;

        }
    }
}
