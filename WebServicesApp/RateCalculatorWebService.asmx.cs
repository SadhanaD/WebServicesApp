using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web.Services;
using System.Xml;
using WebServicesApp.Model;

namespace WebServicesApp
{
    /// <summary>
    /// Summary description for RateCalculatorWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class RateCalculatorWebService : System.Web.Services.WebService
    {
        [WebMethod]
        public ProductRate GetLowestRateByDate(DateTime periodStart, DateTime periodEnd)
        {
            try
            {
                var result = new ProductRate();

                #region Get exect folder path
                var exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
                Regex appPathMatcher = new Regex(@"(?<!fil)[A-Za-z]:\\+[\S\s]*?(?=\\+bin)");
                var appRoot = appPathMatcher.Match(exePath).Value;
                #endregion

                XmlDocument xml = new XmlDocument();
                var xmlfile = appRoot + @"\data\DataSourceFile.xml";

                DataSet dataSet = new DataSet();
                dataSet.ReadXml(xmlfile, XmlReadMode.InferSchema);

                //convert xml data to list
                //Here, I have taken FinalRate as ProductRate + ProgramRateAdjustment
                List<Record> records = (from DataRow dr in dataSet.Tables[0].Rows
                                        select new Record()
                                        {
                                            Product = dr["Product"].ToString(),
                                            Program = dr["Program"].ToString(),
                                            ProductRate = Convert.ToDouble(dr["ProductRate"]),
                                            ProgramRateAdjustment = (float)Convert.ToDouble(dr["ProgramRateAdjustment"]),
                                            FinalRate = Convert.ToDouble(dr["ProductRate"]) + (float)Convert.ToDouble(dr["ProgramRateAdjustment"]),
                                            PeriodStart = Convert.ToDateTime(dr["PeriodStart"]),
                                            PeriodEnd = Convert.ToDateTime(dr["PeriodEnd"])
                                        }).ToList();

                //Get the lowest available rate
                result = records
                    .Where(d => d.PeriodStart >= periodStart && d.PeriodEnd <= periodEnd)
                    .OrderBy(p => p.FinalRate)
                    .Select(a => new ProductRate()
                    {
                        Program = a.Program,
                        Product = a.Product,
                        Rate = a.ProductRate,
                        RateAdjustment = a.ProgramRateAdjustment,
                    })
                    .FirstOrDefault();

                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
