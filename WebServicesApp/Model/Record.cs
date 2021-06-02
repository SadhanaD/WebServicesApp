using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace WebServicesApp.Model
{
	public class Record
	{
		public string Product { get; set; }
		public double ProductRate { get; set; }
		public double FinalRate { get; set; }
		public string Program { get; set; }
		public float ProgramRateAdjustment { get; set; }
		public DateTime PeriodStart { get; set; }
		public DateTime PeriodEnd { get; set; }
	}

	public class ProductRate
	{
		public string Program { get; set; }
		public string Product { get; set; }
		public float RateAdjustment { get; set; }
		public double Rate { get; set; }
	}


}