using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTest
    {
        private static RateCalculatorServiceReference.RateCalculatorWebServiceSoapClient client;

        [ClassInitialize]
        public static void InitializeClass(TestContext ctx)
        {
            client = new RateCalculatorServiceReference.RateCalculatorWebServiceSoapClient();
        }

        [ClassCleanup]
        public static void CleanupClass()
        {
            client.Close();
        }

        [TestMethod]
        public void TestRateCalculatorByMonth()
        {
            var response = client.GetLowestRateByDate(Convert.ToDateTime("2021-03-01"), Convert.ToDateTime("2021-04-30"));
            Assert.IsTrue(response.Rate == 2.8);
        }

        [TestMethod]
        public void TestRateCalculatorByYear()
        {
            var response = client.GetLowestRateByDate(Convert.ToDateTime("2019/04/01"), Convert.ToDateTime("2021/04/22"));
            Assert.IsTrue(response.Rate == 2.8);
        }
    }
}
