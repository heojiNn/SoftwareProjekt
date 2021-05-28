using System;
using System.Text.RegularExpressions;

using NUnit.Framework;

namespace Team14.Data
{
    [TestFixture]
    class TestProgram
    {

        public static void Main(String[] args)
        {
            string input = "Selbstorganisation !";


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
            //Assert.IsNotNull(str, "Null value input");
            Regex rgx = new Regex(@"^[a-zA-ZöÖäÄüÜ ]+");
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
