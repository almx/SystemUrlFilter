using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SystemUrlFilter.Test
{
	[TestClass]
	public class SystemUrlFilterTest
	{
		[TestMethod]
		public void FilterTest()
		{
			SystemUrlFilter.Filter(new string[] { "https://www.everquest.com/free-to-play" });
		}

		[TestMethod]
		public void GetFilterWithoutWildcardsTest()
		{
			Assert.AreEqual("http://blah.com", SystemUrlFilter.GetFilterWithoutWildcards("*http://blah.com*"));
			Assert.AreEqual("http://blah.com", SystemUrlFilter.GetFilterWithoutWildcards("*http://blah.com"));
			Assert.AreEqual("http://blah.com", SystemUrlFilter.GetFilterWithoutWildcards("http://blah.com*"));
		}
	}
}
